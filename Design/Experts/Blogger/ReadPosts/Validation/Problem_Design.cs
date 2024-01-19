using Core;
using Core.Story;
using Core.Story.Model;
using Experts.Blogger.ReadPosts.Model;

namespace Experts.Blogger.ReadPosts.Validation;

public class Problem_Design(ITestOutputHelper output) : Design<Problem>(output) {
    private void Create() => Unit = new Problem(solution);

    private async Task Act() => await Unit.Run(response, token);

    [Fact]
    public void ItHas_Sockets() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IProblem<Model.Request, Model.Response>>();
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

    public readonly ISolution solution = Substitute.For<ISolution>();
    private readonly Response response = Response.Empty();
}


public static class SolutionExtensions {
    public static ISolution MockPass(this ISolution solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>() { });
        return solution;
    }

    public static ISolution MockFail(this ISolution solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>()
            {
                ValidationResult.Failed("TestErrorCode", "TestErrorMessage")
                //new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
            });
        return solution;
    }

}

