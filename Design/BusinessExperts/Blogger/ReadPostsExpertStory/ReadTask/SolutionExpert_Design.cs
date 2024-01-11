using Common.SolutionExperts.DataModel;
using Core;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class SolutionExpert_Design : Design<SolutionExpert>
{
    private void Create() => Unit = new(solution.Mock);

    private async Task Act() => response = await Unit.Read(request.Mock, Token);

    [Fact]
    public void ItRequires_Plugins()
    {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<ISolutionExpert>();
    }

    [Fact]
    public async void Path_Without_Diversion()
    {
        solution.MockRead();

        Create();

        request.UseInvaliedRequestWithMissingFilters();

        await Act();

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => solution.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await solution.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly SolutionMockBuilder solution = new();
    private readonly Request_MockBuilder request = new();
    private List<Common.ExpertStories.DomainModel.Post> response;

    public SolutionExpert_Design(ITestOutputHelper output) : base(output) { }
}


public class SolutionMockBuilder {
    public readonly ISolution Mock = Substitute.For<ISolution>();

    public List<Post> Results { get; internal set; }

    public SolutionMockBuilder MockRead() {
        Results =
        [
            new Post(0, "Title", "Content", DateTime.UtcNow)
        ];
        Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}
