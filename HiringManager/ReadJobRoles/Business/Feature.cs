using Shared.Business;

namespace HiringManager.ReadJobRoles.Business;

public partial class Feature
{
    public async Task<Response> Run(Request request, CancellationToken token) => new Response()
    {
        JobRoles = await ReadFromDataStoreWorkStep(request)
    };

    public Feature(IRepository repository)
    {
        this.repository = repository;
    }

    private Task<List<JobRole>> ReadFromDataStoreWorkStep(Request request) =>
        repository.Add(request);

    private readonly IRepository repository;
}