using Blogger.ReadPosts.Business;
using Core.Business.ValidationModel;
using NSubstitute;

namespace Tests.Blogger.ReadPosts.Business
{
    public class ValidatorPluginAdapter_MockBuilder
    {
        public readonly IValidatorPluginAdapter Mock = Substitute.For<IValidatorPluginAdapter>();

        public ValidatorPluginAdapter_MockBuilder() => MockPassedValidation();

        public ValidatorPluginAdapter_MockBuilder MockPassedValidation()
        {
            var result = new List<ValidationResult> { ValidationResult.Success() };
            Mock.Validate(default, default).ReturnsForAnyArgs(result);
            return this;
        }

        public ValidatorPluginAdapter_MockBuilder MockFailedValidation()
        {
            var result = new List<ValidationResult> { ValidationResult.Failed("errorCode1", "errorMessage1") };
            Mock.Validate(default, default).ReturnsForAnyArgs(result);
            return this;
        }
    }
}
