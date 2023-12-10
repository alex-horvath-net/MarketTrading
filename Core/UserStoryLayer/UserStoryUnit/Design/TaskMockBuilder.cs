//using NSubstitute;

//namespace Core.UserStoryLayer.UserStoryUnit;

//public class TaskMockBuilder
//{
//    public ITask<Response<Request>> Mock = Substitute.For<ITask<Response<Request>>>();

//    public TaskMockBuilder Stopped()
//    {
//        Mock.WhenForAnyArgs(x => x.Run(default, default)).Do(x => { });
//        return this;
//    }

//    public TaskMockBuilder NonStopped()
//    {
//        Mock.WhenForAnyArgs(x => x.Run(default, default)).Do(x => { });
//        return this;
//    }
//}
