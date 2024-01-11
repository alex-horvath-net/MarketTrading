using Core;
using Core.Sockets.ValidationModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class SolutionExpert_Design : Design<SolutionExpert>
{
    private void Construct() => Unit = new(validationPlugin);

    private async Task Validate() => issues = await Unit.Validate(request, Token);

    [Fact]
    public async void ItRequires_Plugins()
    {
        Construct();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public async void Path_Without_Diversion()
    {
        mockValidationPlugin.MockFailedValidation();
        Construct();
        mockRequest.UseValidRequest();

        await Validate();

        issues.Should().NotBeNullOrEmpty();
        issues.Should().OnlyContain(result => mockValidationPlugin.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
        await mockValidationPlugin.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
    }

    public SolutionExpert_Design(ITestOutputHelper output) : base(output) { }

    private readonly SolutionMockBuilder mockValidationPlugin = new();
    private ISolution validationPlugin => mockValidationPlugin.Mock;
    private readonly Request_MockBuilder mockRequest = new();
    private IEnumerable<ValidationDomainModel> issues;

    private Request request => mockRequest.Mock;
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
