namespace Experts.Blogger;

public record Expert(
    ReadPosts.Business.IUserStory ReadPosts,
    ReadPosts.Business.IUserStory GetPost);
