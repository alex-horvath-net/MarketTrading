using Core.Business.DomainModel;

namespace Blogger.ReadPosts.Business;

public interface IRepositoryPluginAdapter
{
    Task<List<Post>> Read(Request request, CancellationToken cancellation);
}