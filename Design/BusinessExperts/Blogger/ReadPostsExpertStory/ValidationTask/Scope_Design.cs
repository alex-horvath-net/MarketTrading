using Core;
using Core.UserStory;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Scope_Design : Design<Scope>
{
    private void Construct() => Unit = new(validationSocket);

    private async Task Run() => await Unit.Run(response, Token);

    [Fact]
    public void ItHas_Sockets()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IScope<Request, Response>>();
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

    private readonly SolutionExpertMockBuilder mockValidationSocket = new();
    private ISolutionExpert validationSocket => mockValidationSocket.Mock;
    private readonly Response_MockBuilder mockResponse = new();
    private Response response => mockResponse.Mock;


    public Scope_Design(ITestOutputHelper output) : base(output) { }
}


public class SolutionExpertMockBuilder
{
    public ISolutionExpert Mock { get; } = Substitute.For<ISolutionExpert>();


    public SolutionExpertMockBuilder Pass()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<ValidationDomainModel>() { });
        return this;
    }

    public SolutionExpertMockBuilder Fail()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<ValidationDomainModel>() { ValidationDomainModel.Failed("TestErrorCode", "TestErrorMessage") });
        return this;
    }

}

