using Core.Business.DomainModel;
using Core.Business.ValidationModel;

namespace Blogger.ReadPosts.Business;

public class Response
{
    public Request Request { get; }
    public IEnumerable<ValidationResult> ValidationResults { get; set; }
    public List<Post> Posts { get; set; }

    public Response(Request request)
    {
        Request = request;
    }
}