using Core.Application.UserStory.DomainModel;
using Design.Core.Enterprise;
using Users.Blogger.ReadPostsUserStory;
using Users.Blogger.ReadPostsUserStory.ReadTask;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

namespace Design.Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

public class SocketDesign : Design<Socket>
{
    private void Construct() => Unit = new Socket(dataAccessPlugin);

    private async Task Run() => response = await Unit.Read(request, Token);

    [Fact]
    public void ItRequires_Plugins()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IDataAccessSocket>();
    }

    [Fact]
    public async void Path_Without_Diversion()
    {
        Construct();

        mockRequest.UseInvaliedRequestWithMissingFilters();

        await Run();

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => mockDataAccessPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await mockDataAccessPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly IDataAccessPlugin_MockBuilder mockDataAccessPlugin = new();
    private IDataAccessPlugin dataAccessPlugin => mockDataAccessPlugin.Mock;
    private readonly Request_MockBuilder mockRequest = new();
    private Request request => mockRequest.Mock;
    private List<Post> response;

    public SocketDesign(ITestOutputHelper output) : base(output) { }
}
