using NSubstitute;

namespace Design.Core.Enterprise.UserStory;

public class IUserTask_MockBuilder
{
    public IUserTask<RequestCore, ResponseCore<RequestCore>> Mock = Substitute.For<IUserTask<RequestCore, ResponseCore<RequestCore>>>();

    public IUserTask_MockBuilder Terminate()
    {
        Mock.Run(default, default).ReturnsForAnyArgs(true);
        return this;
    }

    public IUserTask_MockBuilder DoNotTerminate()
    {
        Mock.Run(default, default).ReturnsForAnyArgs(false);
        return this;
    }
}