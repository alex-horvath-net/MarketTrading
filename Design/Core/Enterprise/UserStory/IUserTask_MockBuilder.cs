using Core.Enterprise.UserStory;
using NSubstitute;

namespace Design.Core.Enterprise.UserStory;

public class IUserTask_MockBuilder
{
    public IUserTask<Request, Response<Request>> Mock = Substitute.For<IUserTask<Request, Response<Request>>>();

    public IUserTask_MockBuilder Terminate()
    {
        Mock.Run(default, default)
            .ReturnsForAnyArgs(Task.CompletedTask)
            .AndDoes(call => call.Arg<Response<Request>>().Terminated = true);
        
        return this;
    }

    public IUserTask_MockBuilder DoNotTerminate()
    {
        Mock.Run(default, default)
            .ReturnsForAnyArgs(Task.CompletedTask)
            .AndDoes(call => call.Arg<Response<Request>>().Terminated = false);

        return this;
    }
}