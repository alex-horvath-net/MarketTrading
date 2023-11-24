namespace Blogger.ReadPosts.Business;

public class WorkFlow(
    IValidatorPluginAdapter validator,
    IRepositoryPluginAdapter repository) : IFeature
{
    public async Task<Response> Run(Request request, CancellationToken cancellation)
    {
        var response = new Response(request);
        response.ValidationResults = await validator.Validate(request, cancellation);
        if (response.ValidationResults.All(x => x.IsSuccess)) response.Posts = await repository.Read(request, cancellation);
        return response;
    }   
}