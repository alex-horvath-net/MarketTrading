using Common.Models.DomainModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public record Request(string Title, string Content) : Core.UserStory.DomainModel.Request {
}


public record Response() : Response<Request> {
    public IEnumerable<Post>? Posts { get; set; }
}
