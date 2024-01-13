using Core;
using Core.ExpertStory;

namespace Experts.Blogger.ReadPosts.Validation;

public class ExpertTask_Design : Design<ExpertTask> {
    private void Create() => Unit = new ExpertTask(solution);

    private async Task Act() => await Unit.Run(response, Token);

    [Fact]
    public void ItHas_Sockets() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IExpertTask<Request, Response>>();
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

    public ExpertTask_Design(ITestOutputHelper output) : base(output) { }

    public readonly ISolution solution = Substitute.For<ISolution>();
    private readonly Response response = Response.Empty;
}


public static class SolutionExtensions {
    public static ISolution MockPass(this ISolution solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<Core.ExpertStory.StoryModel.Validation>() { });
        return solution;
    }

    public static ISolution MockFail(this ISolution solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<Core.ExpertStory.StoryModel.Validation>()
            {
                Core.ExpertStory.StoryModel.Validation.Failed("TestErrorCode", "TestErrorMessage")
                //new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
            });
        return solution;
    }

}

