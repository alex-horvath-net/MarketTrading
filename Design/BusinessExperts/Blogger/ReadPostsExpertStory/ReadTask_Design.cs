using BusinessExperts.Blogger.ReadPostsExpertStory;
using BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;
using Common;
using Common.Plugins.DataAccess;
using Core.UserStory;
using Design.Core;
using Microsoft.AspNetCore.Builder;
using DataModel = Common.Sockets.DataModel;
using DomainModel = Common.UserStory.DomainModel;
using US = BusinessExperts.Blogger.ReadPostsExpertStory;

namespace Design.BusinessExperts.Blogger.ReadPostsExpertStory;

public class ReadTask_Design : Design<Scope> {
    private void Construct() => Unit = new Scope(readSocket.Mock);

    private async Task Run() => await Unit.Run(response.Mock, Token);

    [Fact]
    public void ItRequires_Sockets() {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IScope<Request, Response>>();

    }

    [Fact]
    public async void ItCan_PopulateResponseWithPosts() {
        response.HasNoPosts();
        readSocket.ProvidesPosts();
        Construct();

        await Run();

        response.Mock.Terminated.Should().BeFalse();
        response.Mock.Posts.Should().NotBeEmpty();
        await readSocket.Mock.Received().Read(Arg.Any<Request>(), Arg.Any<CancellationToken>());
    }

    public ReadTask_Design(ITestOutputHelper output) : base(output) { }

    private readonly IReadSocket_MockBuilder readSocket = new();
    private readonly Response_MockBuilder response = new();
}
public class IReadSocket_MockBuilder {
    public ISolutionExpert Mock { get; } = Substitute.For<ISolutionExpert>();

    public IReadSocket_MockBuilder ProvidesPosts() {
        Mock.Read(Arg.Any<Request>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new() { Id = 1, Title = "Post 1", Content = "Content 1" },
                new() { Id = 2, Title = "Post 2", Content = "Content 2" },
                new() { Id = 3, Title = "Post 3", Content = "Content 3" }
            ]);
        return this;
    }
}

public class ReadSocket_Design : Design<SolutionExpert> {
    private void Construct() => Unit = new(readPlugin.Mock);

    private async Task Run() => response = await Unit.Read(request.Mock, Token);

    [Fact]
    public void ItRequires_Plugins() {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<ISolutionExpert>();
    }

    [Fact]
    public async void Path_Without_Diversion() {
        readPlugin.MockRead();

        Construct();

        request.UseInvaliedRequestWithMissingFilters();

        await Run();

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => readPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await readPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly IReadPlugin_MockBuilder readPlugin = new();
    private readonly Request_MockBuilder request = new();
    private List<DomainModel.Post> response;

    public ReadSocket_Design(ITestOutputHelper output) : base(output) { }
}
public class IReadPlugin_MockBuilder {
    public readonly ISolution Mock = Substitute.For<ISolution>();

    public List<DataModel.Post> Results { get; internal set; }

    public IReadPlugin_MockBuilder MockRead() {
        Results =
        [
            new DataModel.Post(0, "Title", "Content", DateTime.UtcNow)
        ];
        Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}

public class ReadPlugin_Design(ITestOutputHelper output) : Design<Solution>(output) {
    private void Create() => Unit = new Solution(db);

    private async Task Use() => posts = await Unit.Read(title, content, Token);

    [Fact]
    public void ItRequires_Dependecies() {
        db = dbPovider.GetTestDB(true);
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<ISolution>();
    }

    [Fact]
    public async Task ItCan_Read() {
        db = dbPovider.GetTestDB();

        Create();

        await Use();

        posts.Should().NotBeEmpty();
    }


    [Fact]
    public void UseDataBase() {
        var appBuilder = WebApplication.CreateBuilder();
        appBuilder.Services.AddCoreApplication(appBuilder.Configuration, isDevelopment: true);
        var app = appBuilder.Build();

        var db = app.UseDeveloperDataBase();

        db.Posts.Should().NotBeEmpty();
    }

    private List<DataModel.Post>? posts;
    private DB? db;
    private DBProvider dbPovider = new();
    string title = "Title";
    string content = "Content";

}
