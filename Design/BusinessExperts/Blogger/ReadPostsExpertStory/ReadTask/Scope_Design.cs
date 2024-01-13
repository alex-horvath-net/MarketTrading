using BusinessExperts.Blogger.ReadPostsExpertStory;
using Common.Solutions.Data.MainDB.DataModel;
using Core;
using Core.ExpertStory;
using Experts.Blogger.ReadPosts;
using Experts.Blogger.ReadPosts.Read;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Scope_Design : Design<ExpertTask> {
    private void Create() => Unit = new ExpertTask(solution);

    private async Task Act() => await Unit.Run(response, Token);

    [Fact]
    public void ItRequires_Solutions() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IExpertTask<Request, Response>>();
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
            .ReturnsForAnyArgs(new List<Common.Scope.ScopeModel.Post> {
                new(){ Id= 1, Title= "Title1", Content= "Content1",  CreatedAt= DateTime.UtcNow},
                new(){ Id= 2, Title= "Title2", Content= "Content2",  CreatedAt= DateTime.UtcNow},
                new(){ Id= 3, Title= "Title3", Content= "Content3",  CreatedAt= DateTime.UtcNow}
            });
        return solution;
    }
}

