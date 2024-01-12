using Common.Models.ValidationModel;
using Core;
using Core.UserStory;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Scope_Design : Design<Scope> {
    private void Create() => Unit = new Scope(solution);

    private async Task Act() => await Unit.Run(response, Token);

    [Fact]
    public void ItHas_Sockets() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IScope<Request, Response>>();
    }

    [Fact]
    public async void ItCan_ValidateValidRequest() {
        solution.MockPass();
        Create();
        response.MockNoValidations();

        await Act();

        response.Terminated.Should().BeFalse();
        response.Validations.Should().NotContain(x => !x.IsSuccess);
        response.Validations.Should().BeEmpty();
        await solution.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest() {
        solution.MockFail();
        Create();
        response.MockNoValidations();

        await Act();

        response.Terminated.Should().BeTrue();
        response.Validations.Should().Contain(x => !x.IsSuccess);
        await solution.ReceivedWithAnyArgs().Validate(default, default);
    }

    public Scope_Design(ITestOutputHelper output) : base(output) { }

    public readonly ISolution solution = Substitute.For<ISolution>();
    private readonly Response response = Response.Empty;
}


public static class SolutionExtensions {
    public static ISolution MockPass(this ISolution solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationIssue>() { });
        return solution;
    }

    public static ISolution MockFail(this ISolution solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationIssue>()
            {
                new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
            });
        return solution;
    }

}

