
using Common;

namespace HiringManager.ReadJobRoles.Feature;

public partial class Feature
{
    public Task<Response> Run(Request request, CancellationToken token)
    {
        var response = new Response();
        response.JobRoles.Add(new("Aladar"));
        return response.ToTask();
    }
}