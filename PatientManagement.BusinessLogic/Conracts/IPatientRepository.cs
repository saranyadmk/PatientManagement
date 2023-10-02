using Microsoft.AspNetCore.JsonPatch;
using PatientManagement.Filters;
using PatientManagement.Models.Models;

namespace PatientManagement.Repository
{
    public interface IPatientRepository
    {
        Task<List<PatientModel>> GetAllPatientsAsync();
        Task<PatientModel> GetPatientByIdAsync(int id);
        Task<PatientModel> AddNewPatientAsync(PatientModel patient);
        Task UpdatePatientAsync(int id, PatientModel patient);
        Task UpdatePatientPatchAsync(int id, JsonPatchDocument patchDocument);
        Task DeletePatientAsync(int id);
        Task<PaginationFilter> GetPatients(string term, string sort, int page, int limit);
    }
}