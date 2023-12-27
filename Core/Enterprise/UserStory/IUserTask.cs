using NSubstitute;

namespace Core.Enterprise.UserStory;

public interface IUserTask<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    Task<bool> Run(TResponse response, CancellationToken token);

    public class MockBuilder
    {
        public IUserTask<RequestCore, ResponseCore<RequestCore>> Mock = Substitute.For<IUserTask<RequestCore, ResponseCore<RequestCore>>>();

        public MockBuilder Terminate()
        {
            Mock.Run(default, default).ReturnsForAnyArgs(true);
            return this;
        }

        public MockBuilder DoNotTerminate()
        {
            Mock.Run(default, default).ReturnsForAnyArgs(false);
            return this;
        }
    }
}

