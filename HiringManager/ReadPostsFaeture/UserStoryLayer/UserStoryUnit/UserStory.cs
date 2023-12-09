using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

public class UserStory(IEnumerable<ITask> workSteps) : UserStory<Request, Response>(workSteps);