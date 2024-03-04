using Core.Business.Model;

namespace Core.Business;

public class TestStory(
  IPresenter<RequestCore, ResponseCore<RequestCore>> presenter,
  ISettings<SettingsCore> settings,
  ITime time,
  IValidator<RequestCore> validator,
  ILog<TestStory> logger) : UserStory<RequestCore, ResponseCore<RequestCore>, SettingsCore>(presenter, validator, settings, logger, time) {
}
