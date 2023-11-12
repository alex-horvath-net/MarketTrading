using Microsoft.EntityFrameworkCore;
using Shared.Technology;

namespace HiringManager.ReadJobRoles.Technology
{
    public class Repository : Adapters.IRepository
    {
        public async Task<List<Shared.Adapter.JobRole>> Read(string name, CancellationToken token) => await database
            .JobRoles
            .Where(x => x.Name.Contains(name))
            .ToListAsync();

        public Repository(AppDatabase database) => this.database = database;
        private readonly AppDatabase database;

    }
}
