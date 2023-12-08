namespace Blogger.ReadPosts.Tasks;

public class Validation(IValidation validation) : Sys.UserStory.ITask<UserStory.Response>
{
    public async Task Run(UserStory.Response response, CancellationToken cancellation)
    {
        response.Validations = await validation.Validate(response.Request, cancellation);
        response.Stopped = response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface IValidation
{
    Task<IEnumerable<Sys.UserStory.ValidationResult>> Validate(UserStory.Request request, CancellationToken cancellation);
}

//--Test--------------------------------------------------