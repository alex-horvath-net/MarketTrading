using Core.Enterprise.UserStory;
using Design.Core.Enterprise;
using Users.Blogger.ReadPostsUserStory;
using static Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationSocket;
using SUT = Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationSocket;

namespace Design.Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

public class ValidationSocket_Design : Design<SUT>
{
    private void Construct() => Unit = new SUT(validationPlugin);

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

    public ValidationSocket_Design(ITestOutputHelper output) : base(output) { }

    private readonly IValidationPlugin_MockBuilder mockValidationPlugin = new();
    private IValidationPlugin validationPlugin => mockValidationPlugin.Mock;
    private readonly Request_MockBuilder mockRequest = new();
    private IEnumerable<ValidationResult> issues;

    private Request request => mockRequest.Mock;
}


