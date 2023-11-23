using Core.Business.ValidationModel;
using NSubstitute;

namespace Blogger.ReadPosts.Business;

public interface IValidatorPluginAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(Business.IFeature.Request request, CancellationToken cancellation);

    public class MockBuilder
    {
        public readonly IValidatorPluginAdapter Mock = Substitute.For<Business.IValidatorPluginAdapter>();

        public MockBuilder() => MockPassedValidation();

        public MockBuilder MockPassedValidation()
        {
            var result = new List<ValidationResult> { ValidationResult.Success() };
            Mock.Validate(default, default).ReturnsForAnyArgs(result);
            return this;
        }

        public MockBuilder MockFailedValidation()
        {
            var result = new List<ValidationResult> { ValidationResult.Failed("errorCode1", "errorMessage1") };
            Mock.Validate(default, default).ReturnsForAnyArgs(result);
            return this;
        }
    }
}