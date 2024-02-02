namespace Core.Business;

public class Story_Design {
  [Fact]
  public async void Provide_Response() {
    time.Default();
    validation.MockPass();
    var userStory = new TestStory(time, validation, logger);

    var response = await userStory.Run(request, token);

    response.Should().NotBeNull();
    response.MetaData.Request.Should().Be(request);
    response.MetaData.Results.Should().BeEmpty();
  }



  private readonly ITime time = Substitute.For<ITime>();
  private readonly IValidator<RequestCore> validation = Substitute.For<IValidator<RequestCore>>();
  private readonly ILogger<TestStory> logger = Substitute.For<ILogger<TestStory>>();
  private readonly RequestCore request = new();
  private readonly CancellationToken token = CancellationToken.None;
}

public static class Extensions {
  public static ITime Default(this ITime time) {
    var returnThis = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Local);
    time.Now.Returns(returnThis);
    return time;
  }

  public static IValidator<RequestCore> MockPass(this IValidator<RequestCore> solution) {
    solution
        .Validate(default, default)
        .ReturnsForAnyArgs(new List<Result>() { });
    return solution;
  }

  public static IValidator<RequestCore> MockFail(this IValidator<RequestCore> solution) {
    solution
        .Validate(default, default)
        .ReturnsForAnyArgs(new List<Result>()
        {
                new Failed("TestErrorCode", "TestErrorMessage")
          //new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
        });
    return solution;
  }
}
