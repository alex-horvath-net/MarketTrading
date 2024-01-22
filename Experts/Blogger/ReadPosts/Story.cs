using Common.Business;
using Core.Business;
using Common.Business.Model;

namespace Experts.Blogger.ReadPosts;

public class Story(
    IValidation<Story.Request> validator, 
    Story.IRepository repository) : Story<Story.Request, Story.Response>(validator) {

    public override async Task RunCore(Response response, CancellationToken token) {
        response.Posts = await repository.Read(response.Request, token);
    }


    public record Response() : Response<Request> {
        public static Response Empty() => new();
        public IEnumerable<Post>? Posts { get; set; }
    }
    

    public record Request(string Title, string Content) : Core.Business.Request {
        public static Request Empty() => new(default, default);
    }
        

    public interface IRepository {
        Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
    }
}
