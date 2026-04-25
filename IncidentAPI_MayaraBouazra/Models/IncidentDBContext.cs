using Microsoft.EntityFrameworkCore;

namespace IncidentAPI_MayaraBouazra.Models
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Incident> Incidents { get; set; }
    }
}
