using Core.PluginAdapters.ValidationModel;
using NSubstitute;

namespace Blogger.ReadPosts.PluginAdapters;

public interface IValidatorPlugin
{
    Task<IEnumerable<ValidationResult>> Validate(Business.IFeature.Request request, CancellationToken cancellation);

    public class MockBuilder
    {
        public readonly IValidatorPlugin Mock = Substitute.For<IValidatorPlugin>();

        public List<ValidationResult> Results { get; private set; }

        public MockBuilder() => MockFailedValidation();
        public MockBuilder MockFailedValidation()
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