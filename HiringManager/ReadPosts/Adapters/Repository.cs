namespace Blogger.ReadPosts.Adapters;

public class Repository : Business.IRepository
{
    public async Task<List<Shared.Business.DomainModel.Post>> Read(Business.Request request, CancellationToken token)
    {
        var dataModel = await repository.Read(request.Title, request.Content, token);
        var domainModel = dataModel.Select(item => new Shared.Business.DomainModel.Post
        {
            Title = item.Title,
            Content = item.Content
        }).ToList();
        return domainModel;
    }

    public Repository(IRepository repository) => this.repository = repository;
    private readonly IRepository repository;
}

public interface IRepository
{
    Task<List<Shared.Adapter.DataModel.Post>> Read(string title, string content, CancellationToken token);
}

