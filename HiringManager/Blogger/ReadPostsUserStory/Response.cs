using Core.Application.UserStory.DomainModel;
using Core.Enterprise.UserStory;

namespace Users.Blogger.ReadPostsUserStory;

public record Response() : ResponseCore<Request>
{
    public List<Post>? Posts { get; set; }

    public class MockMuilder
    {
        public Response Mock { get; private set; } = new();

        public MockMuilder HasNoPosts()
        {
            WillHaveValidRequest();
            Mock.Posts = null;
            return this;
        }

        public MockMuilder WillHaveValidRequest()
        {
            Mock.Request = new Request.MockBuilder().UseValidRequest().Mock;
            Mock.FeatureEnabled = true;
            Mock.Validations = null;
            return this;
        }

        public MockMuilder HasNoValidations()
        {
            WillHaveValidRequest();
            Mock.Validations = null;
            return this;
        }
    }
}
