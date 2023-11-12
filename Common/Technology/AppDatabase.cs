using Microsoft.EntityFrameworkCore;

namespace Shared.Technology
{
    public class AppDatabase : DbContext
    {
        public DbSet<Adapter.JobRole> JobRoles { get; set; }
    }
}
