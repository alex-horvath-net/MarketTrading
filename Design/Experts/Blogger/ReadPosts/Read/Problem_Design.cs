using Common.Problem.Model;
using Core;
using Core.Story;
using Experts.Blogger.ReadPosts.Model;
namespace Experts.Blogger.ReadPosts.Read;

public class Problem_Design(ITestOutputHelper output) : Design<Problem>(output) {
    private void Create() => Unit = new(solution);

    private async Task Act() => await Unit.Run(response, token);

    [Fact]
    public void ItRequires_Solutions() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IProblem<Request, Response>>();
    }

    [Fact]
    public async void ItCan_PopulatePosts() {
        response = response.MockNoPosts();
        solution.CanReceveRead();
        Create();

        await Act();

        await solution.ReceivedRead();
        response.Posts.Should().NotBeEmpty();
        response.Terminated.Should().BeFalse();
    }

    public readonly ISolution solution = Substitute.For<ISolution>();
    private Response response = Response.Empty();
}


public static class SolutionExtensions {

    public static ISolution CanReceveRead(this ISolution solution) {
        solution
            .Read(default, default)
            .ReturnsForAnyArgs(new List<Post> {
                new(){ Id= 1, Title= "Title1", Content= "Content1",  CreatedAt= DateTime.UtcNow},
                new(){ Id= 2, Title= "Title2", Content= "Content2",  CreatedAt= DateTime.UtcNow},
                new(){ Id= 3, Title= "Title3", Content= "Content3",  CreatedAt= DateTime.UtcNow}
            });
        return solution;
    }

    public static async Task<ISolution> ReceivedRead(this ISolution solution) {
        await solution.ReceivedWithAnyArgs().Read(default, default);
        return solution;
    }
}

