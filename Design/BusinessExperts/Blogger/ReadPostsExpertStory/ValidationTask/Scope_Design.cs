using Core;
using Core.UserStory;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Scope_Design : Design<Scope>
{
    private void Create() => Unit = new(expert.Mock);

    private async Task Act() => await Unit.Run(response.Mock, Token);

    [Fact]
    public void ItHas_Sockets()
    {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IScope<Request, Response>>();
    }

    [Fact]
    public async void ItCan_ValidateValidRequest()
    {
        expert.Pass();
        Create();
        response.HasNoValidations();

        await Act();

        response.Mock.Terminated.Should().BeFalse();
        response.Mock.Validations.Should().NotContain(x => !x.IsSuccess);
        response.Mock.Validations.Should().BeEmpty();
        await expert.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest()
    {
        expert.Fail();
        Create();
        response.HasNoValidations();

        await Act();

        response.Mock.Terminated.Should().BeTrue();
        response.Mock.Validations.Should().Contain(x => !x.IsSuccess);
        await expert.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }
    
    public Scope_Design(ITestOutputHelper output) : base(output) { }

    private readonly SolutionExpertMockBuilder expert = new();
    private readonly ResponseMockBuilder response = new();
}


public class SolutionExpertMockBuilder
{
    public ISolutionExpert Mock { get; } = Substitute.For<ISolutionExpert>();


    public SolutionExpertMockBuilder Pass()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<Validation>() { });
        return this;
    }

    public SolutionExpertMockBuilder Fail()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<Validation>() { Validation.Failed("TestErrorCode", "TestErrorMessage") });
        return this;
    }

}

