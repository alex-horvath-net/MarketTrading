//using NSubstitute;

//namespace Core.UserStoryLayer.UserStoryUnit;

//public class Featrue_MockBuilde
//{
//    public readonly IUserStory<Request, Response> Mock = Substitute.For<IUserStory<Request, Response>>();
//    public Request Request;
//    public CancellationToken Token;

//    public Featrue_MockBuilder() => UseValidRequest().UseNoneCanceledToken();

//    public Featrue_MockBuilder UseValidRequest()
//    {
//        Request = new Request("Title", "Content");
//        Request = Request with { Title = Request.Title, Content = Request.Content };
//        return this;
//    }

//    public Featrue_MockBuilder UseInvalidRequest()
//    {
//        Request = new Request(null, null);
//        Request = Request with { Title = Request.Title, Content = Request.Content };
//        return this;
//    }

//    public Featrue_MockBuilder UseNoneCanceledToken()
//    {
//        Token = CancellationToken.None;
//        return this;
//    }
//}
