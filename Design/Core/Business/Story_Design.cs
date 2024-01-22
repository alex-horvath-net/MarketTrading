using Experts.Blogger.ReadPosts;

namespace Core.Business;

public class Story_Design {
    [Fact]
    public async void Provide_Response() {
        validation.MockPass();
        var userStory = new StoryCore<RequestCore, ResponseCore<RequestCore>>(validation);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.ValidationResults.Should().BeEmpty();
    }



    private readonly IValidation<RequestCore> validation = Substitute.For<IValidation<RequestCore>>();
    private readonly RequestCore request = new();
    private readonly CancellationToken token = CancellationToken.None;
}


public static class Extensions {
    public static IValidation<RequestCore> MockPass(this IValidation<RequestCore> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>() { });
        return solution;
    }

    public static IValidation<RequestCore> MockFail(this IValidation<RequestCore> solution) {
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
