using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Models;
using PatientManagement.Models.Models;
using System.Collections.Generic;

namespace PatientManagement.DataAccess
{
    public class PatientDbContext : IdentityDbContext<ApplicationUser>
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {

        }

        public DbSet<PatientModel> Patients { get; set; }
    }
}