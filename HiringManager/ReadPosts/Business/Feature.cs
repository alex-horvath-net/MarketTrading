
namespace Blogger.ReadPosts.Business;

public class Feature(
    IValidationAdapter validation,
    IDataAccessAdapter dataAccess) : IFeature
{
    public async Task<Response> Run(Request request, CancellationToken cancellation)
    {
        var response = new Response(request);
        response.RequestIssues = await validation.Validate(request, cancellation);
        response.Posts = response.RequestIssues.Any(x => !x.IsSuccess) ? null : await dataAccess.Read(request, cancellation);
        return response;
    }
}

public interface IFeature
{
    Task<Response> Run(Request request, CancellationToken cancellation);
}

public record Request(string Title, string Content);

public record Response(Request Request)
{
    public IEnumerable<Core.Business.ValidationResult> RequestIssues { get; set; }
    public List<Core.Business.Post>? Posts { get; set; }
}

public interface IDataAccessAdapter
{
    Task<List<Core.Business.Post>> Read(Request request, CancellationToken cancellation);
}

public interface IValidationAdapter
{
    Task<IEnumerable<Core.Business.ValidationResult>> Validate(Request request, CancellationToken cancellation);
}