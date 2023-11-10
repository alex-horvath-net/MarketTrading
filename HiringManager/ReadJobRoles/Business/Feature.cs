using Shared.Business;

namespace HiringManager.ReadJobRoles.Business;

public class Feature
{
    public async Task<Response> Run(Request request, CancellationToken token)
    {
        response = new Response();
        await WorkSteps(request, token);
        return response;
    }

    private async Task WorkSteps(Request request, CancellationToken token)
    {
        response.JobRoles = await FindJobRoles(request, token);
    }

    public Feature(IRepository repository) => this.repository = repository;

    private Task<List<JobRole>> FindJobRoles(Request request, CancellationToken token) => repository.Read(request, token);

    private readonly IRepository repository;
    private Response response;
}

