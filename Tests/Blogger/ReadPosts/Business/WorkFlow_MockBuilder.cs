using Blogger.ReadPosts.Business;
using NSubstitute;

namespace Tests.Blogger.ReadPosts.Business;

public class WorkFlow_MockBuilder
{
    public readonly IFeature Mock = Substitute.For<IFeature>();
    public Request Request;
    public CancellationToken Token;

    public WorkFlow_MockBuilder() => UseValidRequest().UseNoneCanceledToken();

    public WorkFlow_MockBuilder UseValidRequest()
    {
        Request = new Request("Title", "Content");
        Request = Request with { Title = Request.Title, Content = Request.Content };
        return this;
    }

    public WorkFlow_MockBuilder UseInvalidRequest()
    {
        Request = new Request(null, null);
        Request = Request with { Title = Request.Title, Content = Request.Content };
        return this;
    }

    public WorkFlow_MockBuilder UseNoneCanceledToken()
    {
        Token = CancellationToken.None;
        return this;
    }
}
