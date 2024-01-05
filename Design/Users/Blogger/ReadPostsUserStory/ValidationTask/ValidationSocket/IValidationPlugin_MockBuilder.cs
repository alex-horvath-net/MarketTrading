using Core.Enterprise.Sockets.Validation;
using static Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationSocket;

namespace Design.Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

public class IValidationPlugin_MockBuilder
{
    public readonly IValidationPlugin Mock = Substitute.For<IValidationPlugin>();

    public List<ValidationFailure> Results { get; private set; }

    public IValidationPlugin_MockBuilder MockFailedValidation()
    {
        Results = new List<ValidationFailure>
            {
                new ValidationFailure("Property", "Code", "Message", "Error")
            };
        Mock.Validate(default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}



