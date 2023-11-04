
namespace HiringManager.ReadJobRoles.Feature;

public partial class Feature
{
    public Task<Response> Run(Request request, CancellationToken token)
    {
        var response = new Response();
        response.JobRoles.Add(new());
        return Task.FromResult(response);
    }
}