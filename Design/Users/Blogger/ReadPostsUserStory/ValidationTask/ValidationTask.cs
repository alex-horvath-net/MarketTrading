using Core.Enterprise.UserStory;
using static Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationTask;

namespace Design.Users.Blogger.ReadPostsUserStory.ValidationTask;

public class IValidationSocket_MockBuilder
{
    public IValidationSocket Mock { get; } = Substitute.For<IValidationSocket>();

    public IValidationSocket_MockBuilder Pass()
    {
        Mock.Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>()
            {
            });
        return this;
    }

    public IValidationSocket_MockBuilder Fail()
    {
        Mock.Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>()
            {
                    ValidationResult.Failed("TestErrorCode", "TestErrorMessage")
            });
        return this;
    }

}
