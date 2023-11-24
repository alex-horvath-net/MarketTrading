namespace Blogger.ReadPosts.Business;

public interface IFeature
{
    Task<Response> Run(Request request, CancellationToken cancellation);
}