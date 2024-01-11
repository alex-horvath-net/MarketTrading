using Common.ExpertStories.DomainModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public record Request(string Title, string Content) : Core.UserStory.DomainModel.Request {
}


public record Response() : Response<Request> {
    public List<Post>? Posts { get; set; }
}
