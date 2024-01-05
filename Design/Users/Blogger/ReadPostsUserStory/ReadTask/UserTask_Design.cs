using Core.Enterprise.UserStory;
using Design.Core.Enterprise;
using Users.Blogger.ReadPostsUserStory;
using Users.Blogger.ReadPostsUserStory.ReadTask;

namespace Design.Users.Blogger.ReadPostsUserStory.ReadTask;

public class UserTask_Design : Design<UserTask>
{
    private void Construct() => Unit = new(dataAccessSocket);

    private async Task Run() => terminated = await Unit.Run(response, Token);

    [Fact]
    public void ItRequires_Sockets()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IUserTask<Request, Response>>();
    }

    [Fact]
    public async void ItCan_PopulateResponseWithPosts()
    {
        mockResponse.HasNoPosts();
        mockDataAccessSocket.ProvidesPosts();
        Construct();

        await Run();

        terminated.Should().BeFalse();
        mockResponse.Mock.Posts.Should().NotBeEmpty();
        await mockDataAccessSocket.Mock.ReceivedWithAnyArgs().Read(default, default);
    }

    public UserTask_Design(ITestOutputHelper output) : base(output) { }

    private readonly IDataAccessSocket_MockBuilder mockDataAccessSocket = new();
    private IDataAccessSocket dataAccessSocket => mockDataAccessSocket.Mock;
    private readonly Response_MockBuilder mockResponse = new();
    private Response response => mockResponse.Mock;
    private bool terminated;
}
