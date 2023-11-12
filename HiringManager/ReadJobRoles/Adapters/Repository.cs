namespace HiringManager.ReadJobRoles.Adapters;

public class Repository : Business.IRepository
{
    public async Task<List<Shared.Business.JobRole>> Read(Business.Request request, CancellationToken token)
    {
        var name = request.Name;
        var adapterModelList = await repository.Read(name, token);
        var businessModelList = adapterModelList.Select(adapterItem => new Shared.Business.JobRole(adapterItem.Name)).ToList();
        return businessModelList;
    }

    public Repository(IRepository repository) => this.repository = repository;
    private readonly IRepository repository;
}

public interface IRepository
{
    Task<List<Shared.Adapter.JobRole>> Read(string name, CancellationToken token);
}

