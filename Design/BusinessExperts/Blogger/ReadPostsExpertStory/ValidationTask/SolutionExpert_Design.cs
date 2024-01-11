using Core;
using Core.Sockets.ValidationModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class SolutionExpert_Design : Design<SolutionExpert>
{
    private void Create() => Unit = new(solution.Mock);

    private async Task Act() => issues = await Unit.Validate(request.Mock, Token);

    [Fact]
    public async void ItRequires_Plugins()
    {
        Create();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public async void Path_Without_Diversion()
    {
        solution.MockFailedValidation();
        Create();
        request.UseValidRequest();

        await Act();

        issues.Should().NotBeNullOrEmpty();
        issues.Should().OnlyContain(result => solution.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
        await solution.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
    }

    public SolutionExpert_Design(ITestOutputHelper output) : base(output) { }

    private readonly SolutionMockBuilder solution = new();
    private readonly RequestMockBuilder request = new();
    private IEnumerable<ValidationDomainModel> issues;
}

public class SolutionMockBuilder {
    public readonly ISolution Mock = Substitute.For<ISolution>();

    public List<ValidationSolutionExpertModel> Results { get; private set; }

    public SolutionMockBuilder MockFailedValidation() {
        Results = new List<ValidationSolutionExpertModel>
            {
                new ValidationSolutionExpertModel("Property", "Code", "Message", "Error")
            };
        Mock.Validate(default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}
