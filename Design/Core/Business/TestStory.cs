using Core.Business.Model;

namespace Core.Business;

public class TestStory(
  IEnumerable<IUserWorkStep<RequestCore, ResponseCore<RequestCore>>> workSteps,
  IPresenter<RequestCore, ResponseCore<RequestCore>> presenter,
  ILog<TestStory> logger,
  ITime time) : UserStory<RequestCore, ResponseCore<RequestCore>>(workSteps, presenter, logger, time) {
}
