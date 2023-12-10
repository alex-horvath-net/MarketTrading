using Core.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.UserStory.UserStoryUnit;

public class UserStory(IEnumerable<ITask> workSteps) : UserStoryCore<Request, Response>(workSteps) , IUserStory
{
}
                                                                                                                                           