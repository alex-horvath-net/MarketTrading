using Core.UserStoryLayer.UserStoryUnit;
using Models.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

public record Response() : Response<Request>()
{
    public List<Post>? Posts { get; set; }
}
