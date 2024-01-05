
namespace Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

public class ValidationSocket(ValidationSocket.IValidationPlugin plugin) : ValidationTask.IValidationSocket
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Validate(request, token);
        var userStoryModel = socketModel.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return userStoryModel;
    }

    public interface IValidationPlugin
    {
        Task<IEnumerable<ValidationFailure>> Validate(Request request, CancellationToken token);

        public class MockBuilder
        {
            public readonly IValidationPlugin Mock = Substitute.For<IValidationPlugin>();

            public List<ValidationFailure> Results { get; private set; }

            public MockBuilder MockFailedValidation()
            {
                Results = new List<ValidationFailure>
            {
                new ValidationFailure("Property", "Code", "Message", "Error")
            };
                Mock.Validate(default, default).ReturnsForAnyArgs(Results);
                return this;
            }
        }
    }

    public class Design : Design<ValidationSocket>
    {
        private void Construct() => Unit = new ValidationSocket(validationPlugin);

        private async Task Validate() => issues = await Unit.Validate(request, Token);

        [Fact]
        public async void ItRequires_Plugins() 
        { 
            Construct();

            Unit.Should().NotBeNull();
        }
            
        [Fact]
        public async void Path_Without_Diversion()
        {
            mockValidationPlugin.MockFailedValidation();
            Construct();
            mockRequest.UseValidRequest();

            await Validate();

            issues.Should().NotBeNullOrEmpty();
            issues.Should().OnlyContain(result => mockValidationPlugin.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
            await mockValidationPlugin.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
        }

        public Design(ITestOutputHelper output) : base(output) { }

        private readonly IValidationPlugin.MockBuilder mockValidationPlugin = new();
        private IValidationPlugin validationPlugin => mockValidationPlugin.Mock;
        private readonly Request.MockBuilder mockRequest = new();
        private IEnumerable<ValidationResult> issues;

        private  Request request => mockRequest.Mock;
    }
}

