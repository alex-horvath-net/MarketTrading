using Common.SolutionExperts.DataModel;
using Core;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class SolutionExpert_Design : Design<SolutionExpert>
{
    private void Construct() => Unit = new(readPlugin.Mock);

    private async Task Run() => response = await Unit.Read(request.Mock, Token);

    [Fact]
    public void ItRequires_Plugins()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<ISolutionExpert>();
    }

    [Fact]
    public async void Path_Without_Diversion()
    {
        readPlugin.MockRead();

        Construct();

        request.UseInvaliedRequestWithMissingFilters();

        await Run();

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => readPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await readPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly SolutionMockBuilder readPlugin = new();
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
