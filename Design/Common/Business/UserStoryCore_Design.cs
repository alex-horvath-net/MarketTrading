using Experts.Blogger.ReadPosts;

namespace Common.Business;

public class UserStoryCore_Design {
    [Fact]
    public async void Provide_Response() {
        validation.MockPass();
        var userStory = new Story<Request, Response<Request>>(validation);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.ValidationResults.Should().BeEmpty();
    }



    private readonly IValidation<Request> validation = Substitute.For<IValidation<Request>>();
    private readonly Request request = new();
    private readonly CancellationToken token = CancellationToken.None;
}


public static class Extensions {
    public static IValidation<Request> MockPass(this IValidation<Request> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>() { });
        return solution;
    }

    public static IValidation<Request> MockFail(this IValidation<Request> solution) {
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
