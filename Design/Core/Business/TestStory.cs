namespace Core.Business;

public class TestStory(
  ISettings<SettingsCore> settings,
  ITime time,
  IValidator<RequestCore> validator,
  ILog<TestStory> logger) : UserStoryCore<RequestCore, ResponseCore<RequestCore>, SettingsCore>(settings, validator, logger, time, nameof(TestStory)) {
}
