using Core.Enterprise.UserStory;
using NSubstitute;

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
            Mock = Substitute.For<Request>("Title", "Content");
            return this;
        }

        public MockBuilder UseInvalidRequest()
        {
            Mock = Substitute.For<Request>(null, null);
            return this;
        }
    }
}