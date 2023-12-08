using Blogger.ReadPosts.Adapters;
using Spec.Blogger_Specification.ReadPosts.BusinessWorkFlow;
using Sys.Adapters;

namespace Spec.Blogger_Specification.ReadPosts.PluginAdapters;

public class ValidationAdapter_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new Validation(validator.Mock);
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
    public readonly IValidation Mock = Substitute.For<IValidation>();

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
