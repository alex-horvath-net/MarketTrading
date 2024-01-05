using Users.Blogger.ReadPostsUserStory;

namespace Design.Users.Blogger.ReadPostsUserStory;

public class Request_MockBuilder
{
    public Request Mock { get; private set; }

    public Request_MockBuilder UseValidRequest()
    {
        Mock = new Request("Title", "Content");
        return this;
    }

    public Request_MockBuilder UseInvaliedRequestWithMissingFilters()
    {
        Mock = new Request(null, null);
        return this;
    }

    public Request_MockBuilder UseInvaliedRequestWithShortFilters()
    {
        Mock = new Request("12", "21");
        return this;
    }
}