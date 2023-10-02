using LazyCache;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PatientManagement.Caching;
using PatientManagement.Filters;
using PatientManagement.Migrations;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using PatientManagement.DataAccess;
using PatientManagement.Models.Models;

namespace PatientManagement.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;
        private readonly ICacheProvider _cacheProvider;

        public PatientRepository(PatientDbContext context, ICacheProvider cacheProvider)
        {
            _context = context;
            _cacheProvider = cacheProvider;
        }

        public async Task<List<PatientModel>> GetAllPatientsAsync()
        {
            if (!_cacheProvider.TryGetValue(CacheKeys.Patient, out List<PatientModel> patients))
            {
                var patientss = await _context.Patients.ToListAsync();

                var cacheEntryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(90),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Size = 1024
                };
                _cacheProvider.Set(CacheKeys.Patient, patientss, cacheEntryOption);
            }

            return patients;
        }

        public async Task<PatientModel> GetPatientByIdAsync(int id)
        {
            if (_cacheProvider.TryGetValue(CacheKeys.Patient, out PatientModel patient))
            {
                return patient;
            }
            patient = await _context.Patients.FirstOrDefaultAsync(patient => patient.Id == id);

            if (patient != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(90),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Size = 1024
                };

                _cacheProvider.Set(CacheKeys.Patient, patient, cacheEntryOptions);
            }

            return patient;
        }

        public async Task<PatientModel> AddNewPatientAsync(PatientModel patient)
        {
            _context.Patients.Add(patient);
            patient.CreatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task UpdatePatientAsync(int id,PatientModel patient)
        {
            _context.Patients.Update(patient);
            patient.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientPatchAsync(int id, JsonPatchDocument patchDocument)
        {
            var patient = await _context.Patients.FindAsync(id);
            if(patient != null)
            {
                patchDocument.ApplyTo(patient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if(patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<PaginationFilter> GetPatients(string term, string sort, int page, int limit)
        {
            IQueryable<PatientModel> patients;
            if (!string.IsNullOrWhiteSpace(term))
                patients = _context.Patients;
            else
            {
                term = term.Trim().ToLower();
                patients = _context.Patients.Where(patient => patient.FirstName.ToLower().Contains(term) || patient.LastName.ToLower().Contains(term));
            }

            PaginationFilter pagedPatientData = new PaginationFilter();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var sortedFields = sort.Split(',');

                StringBuilder orderQueryBuilder = new StringBuilder();
                PropertyInfo[] propertyInfo = typeof(PatientModel).GetProperties();

                foreach (var field in sortedFields)
                {
                    string sortOrder = "ascending";
                    var sortField = field.Trim();

                    if (sortField.StartsWith("-"))
                    {
                        sortField = sortField.TrimStart('-');
                        sortOrder = "descending";
                    }

                    var property = propertyInfo.FirstOrDefault(name => name.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                        continue;
                    orderQueryBuilder.Append($"{property.Name.ToString()} {sortOrder},");
                }

                string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
                if (!string.IsNullOrWhiteSpace(orderQuery))
                {
                    patients = patients.OrderBy(orderQuery);
                }
                else
                {
                    patients = patients.OrderBy(order => order.Id);
                }

                var totalCount = await _context.Patients.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)limit);
                var pagedPatients = await patients.Skip((page - 1) * limit).Take(limit).ToListAsync();

                pagedPatientData = new PaginationFilter
                {
                    Patients = pagedPatients,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                };
            }
            return pagedPatientData;
        }
    }
}
