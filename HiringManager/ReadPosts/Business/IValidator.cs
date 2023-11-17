using NSubstitute;
using Shared.Business;

namespace Blogger.ReadPosts.Business
{
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

            public MockBuilder Failed()
            {
                var request = default(Request);
                var token = CancellationToken.None;
                Mock.Validate(request, token).ReturnsForAnyArgs(ValidationResult.Failed("ErrorCode", "ErrorMessage"));
                return this;
            }
        }       
    }
}