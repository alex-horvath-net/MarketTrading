using Blogger.ReadPosts.PluginAdapters;
using Core.PluginAdapters;

namespace Specifications.Blogger.ReadPosts.PluginAdapters
{
    public class ValidatorPlugin_MockBuilder
    {
        public readonly IValidatorPlugin Mock = Substitute.For<IValidatorPlugin>();

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
}
