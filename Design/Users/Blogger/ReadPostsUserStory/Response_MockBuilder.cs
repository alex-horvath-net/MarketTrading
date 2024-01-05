using Users.Blogger.ReadPostsUserStory;

namespace Design.Users.Blogger.ReadPostsUserStory;

public record Response_MockBuilder
{
    public Response Mock { get; private set; } = new();

    public Response_MockBuilder HasNoPosts()
    {
        WillHaveValidRequest();
        Mock.Posts = null;
        return this;
    }

    public Response_MockBuilder WillHaveValidRequest()
    {
        Mock.Request = new Request_MockBuilder().UseValidRequest().Mock;
        Mock.FeatureEnabled = true;
        Mock.Validations = null;
        return this;
    }

    public Response_MockBuilder HasNoValidations()
    {
        WillHaveValidRequest();
        Mock.Validations = null;
        return this;
    }
}
