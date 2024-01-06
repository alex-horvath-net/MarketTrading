using Core.Enterprise.Sockets.Validation;
using Core.Enterprise.UserStory;
using Design.Core.Enterprise;
using US = Users.Blogger.ReadPostsUserStory;

namespace Design.Users.Blogger.ReadPostsUserStory;

public class ValidationTask_Design : Design<US.ValidationTask>
{
    private void Construct() => Unit = new(validationSocket);

    private async Task Run() => await Unit.Run(response, Token);

    [Fact]
    public void ItHas_Sockets()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IUserTask<US.Request, US.Response>>();
    }

    [Fact]
    public async void ItCan_ValidateValidRequest()
    {
        mockValidationSocket.Pass();
        Construct();
        mockResponse.HasNoValidations();

        await Run();

        mockResponse.Mock.Terminated.Should().BeFalse();
        mockResponse.Mock.Validations.Should().NotContain(x => !x.IsSuccess);
        mockResponse.Mock.Validations.Should().BeEmpty();
        await mockValidationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest()
    {
        mockValidationSocket.Fail();
        Construct();
        mockResponse.HasNoValidations();

        await Run();

        mockResponse.Mock.Terminated.Should().BeTrue();
        mockResponse.Mock.Validations.Should().Contain(x => !x.IsSuccess);
        await mockValidationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }

    private readonly IValidationSocket_MockBuilder mockValidationSocket = new();
    private US.IValidationSocket validationSocket => mockValidationSocket.Mock;
    private readonly Response_MockBuilder mockResponse = new();
    private US.Response response => mockResponse.Mock;


    public ValidationTask_Design(ITestOutputHelper output) : base(output) { }
}

public class IValidationSocket_MockBuilder
{
    public US.IValidationSocket Mock { get; } = Substitute.For<US.IValidationSocket>();

    public IValidationSocket_MockBuilder Pass()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<ValidationResult>() { });
        return this;
    }

    public IValidationSocket_MockBuilder Fail()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<ValidationResult>() { ValidationResult.Failed("TestErrorCode", "TestErrorMessage") });
        return this;
    }

}

public class ValidationSocket_Design : Design<US.ValidationSocket>
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

    public ValidationSocket_Design(ITestOutputHelper output) : base(output) { }

    private readonly IValidationPlugin_MockBuilder mockValidationPlugin = new();
    private US.IValidationPlugin validationPlugin => mockValidationPlugin.Mock;
    private readonly Request_MockBuilder mockRequest = new();
    private IEnumerable<ValidationResult> issues;

    private US.Request request => mockRequest.Mock;
}

public class IValidationPlugin_MockBuilder
{
    public readonly US.IValidationPlugin Mock = Substitute.For<US.IValidationPlugin>();

    public List<ValidationFailure> Results { get; private set; }

    public IValidationPlugin_MockBuilder MockFailedValidation()
    {
        Results = new List<ValidationFailure>
            {
                new ValidationFailure("Property", "Code", "Message", "Error")
            };
        Mock.Validate(default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}


public class ValidationPlugin_Design : Design<US.ValidationPlugin>
{
    private void Construct() => Unit = new();

    private async Task Validate() => issues = await Unit.Validate(request, Token);

    [Fact]
    public void ItHas_NoDependecy()
    {
        Construct();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public async void ItCan_AllowValidRequest()
    {
        Construct();
        mockRequest.UseValidRequest();

        await Validate();

        issues.Should().NotBeNull();
        issues.Should().BeEmpty();
    }

    [Fact]
    public async void ItCan_FindMissingFiltersOfRequest()
    {
        Construct();
        mockRequest.UseInvaliedRequestWithMissingFilters();

        await Validate();

        issues.Should().NotBeNull();
        issues.Should().HaveCount(2);
        issues.Should().ContainSingle(x =>
            x.PropertyName == "Title" &&
            x.ErrorCode == "NotEmptyValidator" &&
            x.ErrorMessage == "'Title' can not be empty if 'Content' is empty." &&
            x.Severity == "Error");

        issues.Should().ContainSingle(x =>
            x.PropertyName == "Content" &&
            x.ErrorCode == "NotEmptyValidator" &&
            x.ErrorMessage == "'Content' can not be empty if 'Title' is empty." &&
            x.Severity == "Error");
    }

    [Fact]
    public async void ItCan_FindShortFiltersOfRequest()
    {
        Construct();
        mockRequest.UseInvaliedRequestWithShortFilters();

        await Validate();

        issues.Should().NotBeNull();
        issues.Should().HaveCount(2);
        issues.Should().ContainSingle(x =>
            x.PropertyName == "Title" &&
            x.ErrorCode == "MinimumLengthValidator" &&
            x.ErrorMessage == "The length of 'Title' must be at least 3 characters. You entered 2 characters." &&
            x.Severity == "Error");

        issues.Should().ContainSingle(x =>
            x.PropertyName == "Content" &&
            x.ErrorCode == "MinimumLengthValidator" &&
            x.ErrorMessage == "The length of 'Content' must be at least 3 characters. You entered 2 characters." &&
            x.Severity == "Error");
    }

    private readonly Request_MockBuilder mockRequest = new();
    private US.Request request => mockRequest.Mock;
    private IEnumerable<ValidationFailure> issues;

    public ValidationPlugin_Design(ITestOutputHelper output) : base(output) { }
}

