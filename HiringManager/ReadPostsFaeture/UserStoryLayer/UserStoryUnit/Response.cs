using Common.UserStoryLayer.UserStoryUnit;
using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

public record Response() : Response<Request>()
{
    public List<Post>? Posts { get; set; }
}
