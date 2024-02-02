namespace Core.Business;

public class TestStory(
  ITime time,
  IValidator<RequestCore> validator,
  ILogger<TestStory> logger) : StoryCore<RequestCore, ResponseCore<RequestCore>>(time, validator, logger, nameof(TestStory)) {
}
