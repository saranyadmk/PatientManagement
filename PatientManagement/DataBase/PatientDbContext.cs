using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Models;

namespace PatientManagement.DataBase
{
    public class PatientDbContext : IdentityDbContext<ApplicationUser>
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) 
        {
            
        }

        public DbSet<PatientModel> Patients { get; set; }
    }
}
