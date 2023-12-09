using NSubstitute;

namespace Principals.UserStoryLayer.UserStoryUnit;

public interface ITask<TResponse> where TResponse : Response<Request>
{
    Task Run(TResponse response, CancellationToken cancellation);

    public class MockBuilder
    {
        public ITask<Response<Request>> Mock = Substitute.For<ITask<Response<Request>>>();

        public MockBuilder Stopped()
        {
            Mock.WhenForAnyArgs(x => x.Run(default, default)).Do(x => { });
            return this;
        }

        public MockBuilder NonStopped()
        {
            Mock.WhenForAnyArgs(x => x.Run(default, default)).Do(x => { });
            return this;
        }
    }
}
