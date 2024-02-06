using Core.Business.Model;
using Experts.Blogger.ReadPosts;

namespace Core.Business;

public class Story_Design {
  [Fact]
  public async void Provide_Response() {
    settings.Enabled();
    time.Freeze_2023_01_01();
    validation.MockPass();
    var userStory = new TestStory(settings, time, validation, logger);

    var response = await userStory.Run(request, token);

    response.Should().NotBeNull();
    response.MetaData.Request.Should().Be(request);
    response.MetaData.Results.Should().BeEmpty();
  }


  private readonly ISettings<SettingsCore> settings = Substitute.For<ISettings<SettingsCore>>();
  private readonly ITime time = Substitute.For<ITime>();
  private readonly ILog<TestStory> logger = Substitute.For<ILog<TestStory>>();
  private readonly IValidator<RequestCore> validation = Substitute.For<IValidator<RequestCore>>();
  private readonly RequestCore request = new();
  private readonly CancellationToken token = CancellationToken.None;
}

public static class Extensions {
  public static ISettings<SettingsCore> Enabled(this ISettings<SettingsCore > settings) {
    var returnThis = new SettingsCore("") { Enabled = true };
    settings.Value.Returns(returnThis);
    return settings;
  }

  public static ISettings<SettingsCore> Disabled(this ISettings<SettingsCore> settings) {
    var returnThis = new SettingsCore("") { Enabled = false };
    settings.Value.Returns(returnThis);
    return settings;
  }

  public static ITime Freeze_2023_01_01(this ITime time) {
    var returnThis = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    time.UtcNow.Returns(returnThis);
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
