namespace Core.Business;

public class Story_Design {
    [Fact]
    public async void Provide_Response() {
        validation.MockPass();
        var userStory = new TestStory(validation, logger);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.ValidationResults.Should().BeEmpty();
    }



    private readonly IValidator<RequestCore> validation = Substitute.For<IValidator<RequestCore>>();
    private readonly ILogger<TestStory> logger = Substitute.For<ILogger<TestStory>>();
    private readonly RequestCore request = new();
    private readonly CancellationToken token = CancellationToken.None;
}

public static class Extensions {
    public static IValidator<RequestCore> MockPass(this IValidator<RequestCore> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>() { });
        return solution;
    }

    public static IValidator<RequestCore> MockFail(this IValidator<RequestCore> solution) {
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
