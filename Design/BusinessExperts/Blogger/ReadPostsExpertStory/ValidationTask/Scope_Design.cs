using Common.Models.ValidationModel;
using Core;
using Core.UserStory;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Scope_Design : Design<Scope>
{
    private void Create() => Unit = new Scope(solution.Mock);

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
        solution.Pass();
        Create();
        response.HasNoValidations();

        await Act();

        response.Mock.Terminated.Should().BeFalse();
        response.Mock.Validations.Should().NotContain(x => !x.IsSuccess);
        response.Mock.Validations.Should().BeEmpty();
        await solution.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest()
    {
        solution.Fail();
        Create();
        response.HasNoValidations();

        await Act();

        response.Mock.Terminated.Should().BeTrue();
        response.Mock.Validations.Should().Contain(x => !x.IsSuccess);
        await solution.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }
    
    public Scope_Design(ITestOutputHelper output) : base(output) { }

    private readonly SolutionMockBuilder solution = new();
    private readonly ResponseMockBuilder response = new();
}


public class SolutionMockBuilder
{
    public ISolution Mock { get; } = Substitute.For<ISolution>();


    public SolutionMockBuilder Pass()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<ValidationIssue>() { });
        return this;
    }

    public SolutionMockBuilder Fail()
    {
        Mock.Validate(default, default).ReturnsForAnyArgs(new List<ValidationIssue>() 
        {
            new ValidationIssue("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity") 
        });
        return this;
    }

}

//public class SolutionMockBuilder {
//    public readonly ISolution Mock = Substitute.For<ISolution>();

//    public List<ValidationSolutionExpertModel> Results { get; private set; }

//    public SolutionMockBuilder MockFailedValidation() {
//        Results = new List<ValidationSolutionExpertModel>
//            {
//                new ValidationSolutionExpertModel("Property", "Code", "Message", "Error")
//            };
//        Mock.Validate(default, default).ReturnsForAnyArgs(Results);
//        return this;
//    }
//}
