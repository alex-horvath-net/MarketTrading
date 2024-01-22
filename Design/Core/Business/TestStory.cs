namespace Core.Business;

public class TestStory(
    IValidator<RequestCore> validator,
    ILogger<TestStory> logger) : StoryCore<RequestCore, ResponseCore<RequestCore>, TestStory>(validator, logger) {
}
