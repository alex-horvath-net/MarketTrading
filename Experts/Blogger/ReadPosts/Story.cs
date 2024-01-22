using Common.Model;

namespace Experts.Blogger.ReadPosts;

public class Story(
    Story.IValidation validator, 
    Story.IRepository repository) : Common.Story<Story.Request, Story.Response>() {

    public override async Task RunCore(Response response, CancellationToken token) {
        response.ValidationResults = await validator.Validate(response.Request, token);
        response.Terminated = response.ValidationResults.Any(x => !x.IsSuccess);
        if (response.Terminated)
            return;
        response.Posts = await repository.Read(response.Request, token);
    }

    public record Request(string Title, string Content) : global::Common.Model.Request {
        public static Request Empty() => new(default, default);
    }


    public record Response() : Response<Request> {
        public static Response Empty() => new();
        public IEnumerable<Post>? Posts { get; set; }
    }


    public interface IValidation {
        Task<IEnumerable<ValidationResult>> Validate(Story.Request request, CancellationToken token);
    }


    public interface IRepository {
        Task<IEnumerable<Common.Model.Post>> Read(Request Request, CancellationToken token);
    }
}
