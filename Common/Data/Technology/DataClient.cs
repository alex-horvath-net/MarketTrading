using System.Linq.Expressions;
using Common.Data.Adapters;
using Microsoft.EntityFrameworkCore;

namespace Common.Data.Technology;
public class DataClient<T>(AppDB db) : IDataClient<T> where T : class {

    public async Task<T> Add(T dataModel, CancellationToken token) {
        db.Add(dataModel);
        await db.SaveChangesAsync(token);
        return dataModel;
    }


    public async Task<T> Remove(T dataModel, CancellationToken token) {
        db.Remove(dataModel);
        await db.SaveChangesAsync(token);
        return dataModel;
    }

    public async Task<T> Update(T dataModel, CancellationToken token) {
        db.Update(dataModel);
        await db.SaveChangesAsync(token);
        return dataModel;
    }

    public async Task<T?> Find(long id, CancellationToken token) {
        return await db.FindAsync<T>(id, token);
    }

    public async Task<List<T>> Find(Expression<Func<T, bool>> predicate, CancellationToken token) {
        return await db.Set<T>().Where(predicate).ToListAsync(token);
    }

    public async Task<List<T>> Find(CancellationToken token) {
        return await db.Set<T>().ToListAsync(token);
    }
}
 