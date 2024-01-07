using Core.App;
using Core.App.Plugins;
using Core.Sys.UserStory;
using Design.Core.Sys;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataModel = Core.App.Sockets.DataModel;
using DomainModel = Core.App.UserStory.DomainModel;
using US = Users.Blogger.ReadPostsUserStory;

namespace Design.Users.Blogger.ReadPostsUserStory;


public class ReadTask_Design : Design<US.ReadTask>
{
    private void Construct() => Unit = new US.ReadTask(readSocket);

    private async Task Run() => await Unit.Run(response, Token);

    [Fact]
    public void ItRequires_Sockets()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IUserTask<US.Request, US.Response>>();
        
    }

    [Fact]
    public async void ItCan_PopulateResponseWithPosts()
    {
        mockResponse.HasNoPosts();
        mockReadSocket.ProvidesPosts();
        Construct();

        await Run();

        mockResponse.Mock.Terminated.Should().BeFalse();
        mockResponse.Mock.Posts.Should().NotBeEmpty();
        await mockReadSocket.Mock.ReceivedWithAnyArgs().Read(default, default);
    }

    public ReadTask_Design(ITestOutputHelper output) : base(output) { }

    private readonly IReadSocket_MockBuilder mockReadSocket = new();
    private US.IReadSocket readSocket => mockReadSocket.Mock;
    private readonly Response_MockBuilder mockResponse = new();
    private US.Response response => mockResponse.Mock;
}

public class IReadSocket_MockBuilder
{
    public US.IReadSocket Mock { get; } = Substitute.For<US.IReadSocket>();

    public IReadSocket_MockBuilder ProvidesPosts()
    {
        Mock.Read(Arg.Any<US.Request>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new() { Id = 1, Title = "Post 1", Content = "Content 1" },
                new() { Id = 2, Title = "Post 2", Content = "Content 2" },
                new() { Id = 3, Title = "Post 3", Content = "Content 3" }
            ]);
        return this;
    }
}

public class ReadSocket_Design : Design<US.ReadSocket>
{
    private void Construct() => Unit = new (readPlugin);

    private async Task Run() => response = await Unit.Read(request, Token);

    [Fact]
    public void ItRequires_Plugins()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<US.IReadSocket>();
    }

    [Fact]
    public async void Path_Without_Diversion()
    {
        mockReadPlugin.MockRead();

        Construct();

        mockRequest.UseInvaliedRequestWithMissingFilters();

        await Run();

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => mockReadPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await mockReadPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly IReadPlugin_MockBuilder mockReadPlugin = new();
    private US.IReadPlugin readPlugin => mockReadPlugin.Mock;
    private readonly Request_MockBuilder mockRequest = new();
    private US.Request request => mockRequest.Mock;
    private List<DomainModel.Post> response;

    public ReadSocket_Design(ITestOutputHelper output) : base(output) { }
}

public class IReadPlugin_MockBuilder
{
    public readonly US.IReadPlugin Mock = Substitute.For<US.IReadPlugin>();

    public List<DataModel.Post> Results { get; internal set; }

    public IReadPlugin_MockBuilder MockRead()
    {
        Results =
        [
            new() { Title = "Title", Content = "Content" }
        ];
        Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}

public class PluginDesign
{
    [Fact]
    public async void Initialize()
    {
        var options = new DbContextOptions<BlogDbContext>();
        var db = new BlogDbContext(options);
        db.EnsureInitialized();
        db.EnsureInitialized();

        var unit = new US.ReadPlugin(db);
        var title = "Title";
        var content = "Content";
        var response = await unit.Read(title, content, CancellationToken.None);

        response.Should().OnlyContain(post => post.Title.Contains(title));
    }

    [Fact]
    public void UseDataBase()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddCommon(builder.Configuration);
        var app = builder.Build();

        app.UseDataBase();
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

        db.Posts.Should().NotBeEmpty();
    }
}
