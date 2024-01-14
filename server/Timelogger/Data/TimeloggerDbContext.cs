using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger.Data
{
    public class TimeloggerDbContext : DbContext
    {
        public TimeloggerDbContext(DbContextOptions<TimeloggerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<TimeRegistration> TimeRegistrations { get; set; }
    }
}