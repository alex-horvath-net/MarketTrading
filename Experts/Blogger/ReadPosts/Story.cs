using Common.Business;
using Common.Business.Model;

namespace Experts.Blogger.ReadPosts;

public class Story(
    Story.IValidation validator, 
    Story.IRepository repository) : Story<Story.Request, Story.Response>() {

    public override async Task RunCore(Response response, CancellationToken token) {
        response.ValidationResults = await validator.Validate(response.Request, token);
        response.Terminated = response.ValidationResults.Any(x => !x.IsSuccess);
        if (response.Terminated)
            return;
        response.Posts = await repository.Read(response.Request, token);
    }

    public record Response() : Response<Request> {
        public static Response Empty() => new();
        public IEnumerable<Post>? Posts { get; set; }
    }
    
    public record Request(string Title, string Content) : global::Common.Business.Request {
        public static Request Empty() => new(default, default);
    }

    
    public interface IValidation {
        Task<IEnumerable<ValidationResult>> Validate(Story.Request request, CancellationToken token);
    }


    public interface IRepository {
        Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
    }
}
