using Shared.Business;
using Shared.Business.DomainModel;

namespace Blogger.ReadPosts.Business;

public class Response
{
    public Response(Request request)
    {
        Request = request;
    }

    public List<Post> Posts { get; set; }
    public Request Request { get;  }
    public ValidationResult ValidationResult { get; internal set; }
}
