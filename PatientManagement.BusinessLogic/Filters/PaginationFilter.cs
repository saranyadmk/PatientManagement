using PatientManagement.Models.Models;

namespace PatientManagement.Filters
{
    public class PaginationFilter
    {
        public IEnumerable<PatientModel> Patients { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
