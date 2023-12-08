namespace Sys.UserStory;

public interface ITask<T>
{
    Task Run(T response, CancellationToken cancellation);

    public class MockBuilder
    {

    }
}