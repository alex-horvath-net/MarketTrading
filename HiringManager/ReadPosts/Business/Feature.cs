
namespace Blogger.ReadPosts.Business;

public class Feature(
    IValidationAdapter validation,
    IDataAccessAdapter dataAccess) : IFeature
{
    public async Task<Response> Run(Request request, CancellationToken cancellation)
    {
        var response = new Response() { Request = request };
        response.ValidationResults = await validation.Validate(request, cancellation);
        if (response.ValidationResults.All(x => x.IsSuccess)) response.Posts = await dataAccess.Read(request, cancellation);
        return response;
    }
}

public interface IFeature
{
    Task<Response> Run(Request request, CancellationToken cancellation);
}

public record Request(string Title, string Content)
{
}

public class Response
{
    public Request Request { get; set; }
    public IEnumerable<Core.Business.ValidationResult> ValidationResults { get; set; }
    public List<Core.Business.Post> Posts { get; set; }
}


public interface IDataAccessAdapter
{
    Task<List<Core.Business.Post>> Read(Request request, CancellationToken cancellation);
}

public interface IValidationAdapter
{
    Task<IEnumerable<Core.Business.ValidationResult>> Validate(Request request, CancellationToken cancellation);
}