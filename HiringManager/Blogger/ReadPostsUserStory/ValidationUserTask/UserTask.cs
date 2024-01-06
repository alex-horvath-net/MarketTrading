using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ValidationUserTask.ValidationSocket;

namespace Users.Blogger.ReadPostsUserStory.ValidationUserTask;

public class UserTask(UserTask.IValidationSocket socket) : IUserTask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }

    public interface IValidationSocket
    {
        Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);
    }
}

public static class UserTaskExtensions
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services) => services
        .AddScoped<IUserTask<Request, Response>, UserTask>()
        .AddValidationSocket();
}