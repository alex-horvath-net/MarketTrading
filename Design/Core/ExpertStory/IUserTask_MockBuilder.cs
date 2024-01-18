using Core.ExpertStory.StoryModel;
using NSubstitute;

namespace Core.ExpertStory;

public class IUserTask_MockBuilder {
    public IProblem<Request, Response<Request>> Mock = Substitute.For<IProblem<Request, Response<Request>>>();

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