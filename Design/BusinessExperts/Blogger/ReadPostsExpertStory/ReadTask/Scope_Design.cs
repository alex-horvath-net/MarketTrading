using BusinessExperts.Blogger.ReadPosts;
using BusinessExperts.Blogger.ReadPosts.ReadTask;
using Common.Solutions.Data.MainDB.DataModel;
using Core;
using Core.ExpertStory;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Scope_Design : Design<Scope> {
    private void Create() => Unit = new Scope(solution);

    private async Task Act() => await Unit.Run(response, Token);

    [Fact]
    public void ItRequires_Solutions() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IScope<Request, Response>>();
    }

    [Fact]
    public async void ItCan_PopulatePosts() {
        response = response.MockNoPosts();
        solution.MockPosts();
        Create();

        await Act();

        response.Terminated.Should().BeFalse();
        response.Posts.Should().NotBeEmpty();
        await solution.ReceivedWithAnyArgs().Read(default, default);
    }

    public Scope_Design(ITestOutputHelper output) : base(output) { }

    public readonly ISolution solution = Substitute.For<ISolution>();
    private Response response = Response.Empty;
}


public static class SolutionExtensions {

    public static ISolution MockPosts(this ISolution solution) {
        solution
            .Read(default, default)
            .ReturnsForAnyArgs(new List<Post> {
                new(1, "Title1", "Content1", DateTime.UtcNow),
                new(2, "Title2", "Content2", DateTime.UtcNow),
                new(3, "Title3", "Content3", DateTime.UtcNow)
            });
        return solution;
    }
}

