
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadUserTask;
using Users.Blogger.ReadPostsUserStory.ValidationUserTask.ValidationSocket.ValidationPlugin;

namespace Users.Blogger.ReadPostsUserStory.ValidationUserTask.ValidationSocket;

public class Socket(Socket.IValidationPlugin plugin) : UserTask.IValidationSocket
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Validate(request, token);
        var userStoryModel = socketModel.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return userStoryModel;
    }

    public interface IValidationPlugin
    {
        Task<IEnumerable<ValidationFailure>> Validate(Request request, CancellationToken token);
    }
}

public static class SocketExtensions
{
    public static IServiceCollection AddValidationSocket(this IServiceCollection services) => services
        .AddScoped<UserTask.IValidationSocket, Socket>()
        .AddValidationPlugin();
}

