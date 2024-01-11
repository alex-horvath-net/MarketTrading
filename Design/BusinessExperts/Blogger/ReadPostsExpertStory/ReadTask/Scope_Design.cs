using Core;
using Core.UserStory;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Scope_Design : Design<Scope> {
    private void Create() => Unit = new Scope(expert.Mock);

    private async Task Act() => await Unit.Run(response.Mock, Token);

    [Fact]
    public void ItRequires_Experts() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IScope<Request, Response>>();

    }

    [Fact]
    public async void ItCan_PopulateResponseWithPosts() {
        response.HasNoPosts();
        expert.ProvidesPosts();
        Create();

        await Act();

        response.Mock.Terminated.Should().BeFalse();
        response.Mock.Posts.Should().NotBeEmpty();
        await expert.Mock.Received().Read(Arg.Any<Request>(), Arg.Any<CancellationToken>());
    }

    public Scope_Design(ITestOutputHelper output) : base(output) { }

    private readonly SolutionExpertMockBuilder expert = new();
    private readonly ResponseMockBuilder response = new();
}


public class SolutionExpertMockBuilder {
    public ISolutionExpert Mock { get; } = Substitute.For<ISolutionExpert>();

    public SolutionExpertMockBuilder ProvidesPosts() {
        Mock.Read(Arg.Any<Request>(), Arg.Any<CancellationToken>())
            .Returns([
                new() { Id = 1, Title = "Post 1", Content = "Content 1" },
                new() { Id = 2, Title = "Post 2", Content = "Content 2" },
                new() { Id = 3, Title = "Post 3", Content = "Content 3" }]);
        return this;
    }
}
