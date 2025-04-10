using System.Linq.Expressions;
using Infrastructure.Adapters.App.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Technology.EF;
public class CommonEFClient<TDatabase, TDataModel>(TDatabase db) : ICommonEFClient<TDataModel> where TDatabase : DbContext where TDataModel : class {
    protected TDatabase DB => db;

    public async Task<TDataModel> Add(TDataModel dataModel, CancellationToken token) {
        DB.Add(dataModel);
        await DB.SaveChangesAsync(token);
        return dataModel;
    }


    public async Task<TDataModel> Remove(TDataModel dataModel, CancellationToken token) {
        DB.Remove(dataModel);
        await DB.SaveChangesAsync(token);
        return dataModel;
    }

    public async Task<TDataModel> Update(TDataModel dataModel, CancellationToken token) {
        DB.Update(dataModel);
        await DB.SaveChangesAsync(token);
        return dataModel;
    }

    public async Task<TDataModel?> Find(long id, CancellationToken token) {
        return await DB.FindAsync<TDataModel>(id, token);
    }

    public async Task<List<TDataModel>> Find(Expression<Func<TDataModel, bool>> predicate, CancellationToken token) {
        return await DB.Set<TDataModel>().Where(predicate).ToListAsync(token);
    }

    public async Task<List<TDataModel>> Find(CancellationToken token) {
        return await DB.Set<TDataModel>().ToListAsync(token);
    }
}

