using Core.Business.DomainModel;
using Core.Business.ValidationModel;
using NSubstitute;

namespace Blogger.ReadPosts.Business;

public interface IFeature
{
    Task<Response> Run(Request request, CancellationToken cancellation);

    public record Request(string Title, string Content)
    {
    }

    public class Response
    {
        public Response(Request request)
        {
            Request = request;
        }

        public List<Post> Posts { get; set; }
        public Request Request { get; }
        public IEnumerable<ValidationResult> ValidationResults { get; internal set; }
    }

    public class MockBuilder
    {
        public readonly IFeature Mock = Substitute.For<IFeature>();
        public IFeature.Request Request;
        public CancellationToken Token;

        public MockBuilder() => UseDefaultRequest().UseDefaultToken();

        public MockBuilder UseDefaultRequest()
        {
            Request = new IFeature.Request("Title", "Content");
            return this;
        }

        public MockBuilder UseDefaultToken()
        {
            Token = CancellationToken.None;
            return this;
        }
    }
}