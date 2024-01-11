using Core.UserStory.DomainModel;
using NSubstitute;

namespace Core.UserStory;

public class IUserTask_MockBuilder {
    public IScope<Request, Response<Request>> Mock = Substitute.For<IScope<Request, Response<Request>>>();

    public IUserTask_MockBuilder Terminate() {
        Mock.Run(default, default)
            .ReturnsForAnyArgs(Task.CompletedTask)
            .AndDoes(call => call.Arg<Response<Request>>().Terminated = true);

        return this;
    }

    public IUserTask_MockBuilder DoNotTerminate() {
        Mock.Run(default, default)
            .ReturnsForAnyArgs(Task.CompletedTask)
            .AndDoes(call => call.Arg<Response<Request>>().Terminated = false);

        return this;
    }
}