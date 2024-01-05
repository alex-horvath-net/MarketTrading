namespace Users.Blogger.ReadPostsUserStory;

public record Response() : ResponseCore<Request>
{
    public List<DomainModel.Post>? Posts { get; set; }
}
