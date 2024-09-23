using Microsoft.EntityFrameworkCore;
using SiyafundaApplication.Models;

namespace SiyafundaApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LeadEntity> Leads { get; set; }
    }
}
