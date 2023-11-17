using NSubstitute;
using Shared.Business;

namespace Blogger.ReadPosts.Business;

public interface IValidator
{
    Task<ValidationResult> Validate(Request request, CancellationToken token);

    public class MockBuilder
    {
        public readonly IValidator Mock = Substitute.For<IValidator>();

        public MockBuilder Success()
        {
            var request = default(Request);
            var token = CancellationToken.None;
            Mock.Validate(request, token).ReturnsForAnyArgs(ValidationResult.Success());
            return this;
        }

        public MockBuilder Failed() => Failed("ErrorCode", "ErrorMessage");
        public MockBuilder Failed(string errorCode, string errorMessage)
        {
            var request = default(Request);
            var token = CancellationToken.None;
            Mock.Validate(request, token).ReturnsForAnyArgs(ValidationResult.Failed(errorCode, errorMessage));
            return this;
        }
    }
}