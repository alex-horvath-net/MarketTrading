using Blogger.UserStories.ReadPosts.Design;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask.Sockets.ValidationSocket;
using Core.Sockets.Validation;
using FluentAssertions;
using NSubstitute;

namespace Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket.Design;

public class ValidationAdapter_Specification
{
    //[Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new ValidationSocket(validator.Mock);
        var response = await unit.Validate(feature.Request, feature.Token);

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => validator.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
        await validator.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
    }

    private readonly ValidatorPlugin_MockBuilder validator = new();
    private readonly Featrue_MockBuilder feature = new();
}

public class ValidatorPlugin_MockBuilder
{
    public readonly IValidationPlugin Mock = Substitute.For<IValidationPlugin>();

    public List<ValidationResult> Results { get; private set; }

    public ValidatorPlugin_MockBuilder() => MockFailedValidation();
    public ValidatorPlugin_MockBuilder MockFailedValidation()
    {
        Results = new List<ValidationResult>
            {
                new ValidationResult("Property", "Code", "Message", "Error")
            };
        Mock.Validate(default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}
