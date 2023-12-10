//namespace Core.UserStoryCore.UserStoryUnit;

//public class Tasks_MockBuilder
//{
//    public readonly List<ITask<Response>> Mock = new List<ITask<Response>>();

//    public Tasks_MockBuilder() => UseNonStoppedWorkSteps();

//    public Tasks_MockBuilder UseNonStoppedWorkSteps()
//    {
//        Mock.Clear();
//        Mock.Add(new ContinueWorkStep());
//        Mock.Add(new ContinueWorkStep());
//        Mock.Add(new ContinueWorkStep());
//        return this;
//    }

//    public Tasks_MockBuilder MockStoppedWorkSteps()
//    {
//        Mock.Clear();
//        Mock.Add(new ContinueWorkStep());
//        Mock.Add(new StopWorkStep());
//        Mock.Add(new ContinueWorkStep());
//        return this;
//    }

//    public class StopWorkStep : ITask<Response>
//    {
//        public Task Run(Response response, CancellationToken cancellation)
//        {
//            response.Stopped = true;
//            return Task.CompletedTask;
//        }
//    }

//    public class ContinueWorkStep : ITask<Response>
//    {
//        public Task Run(Response response, CancellationToken cancellation)
//        {
//            response.Stopped = false;
//            return Task.CompletedTask;
//        }
//    }
//}