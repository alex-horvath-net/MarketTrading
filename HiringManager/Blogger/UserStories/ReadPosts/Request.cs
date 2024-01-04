using Core.Enterprise.UserStory;

namespace Users.Blogger.UserStories.ReadPosts;

public record Request(
    string Title,
    string Content) : RequestCore
{
    public class MockBuilder
    {
        public Request Mock { get; private set; }

        public MockBuilder UseValidRequest()
        {
            Mock = new Request("Title", "Content");
            return this;
        }

        public MockBuilder UseInvaliedRequestWithMissingFilters()
        { 
            Mock = new Request(null, null);
            return this;
        }

        public MockBuilder UseInvaliedRequestWithShortFilters()
        {
            Mock = new Request("12", "21");
            return this;
        }
    }
}