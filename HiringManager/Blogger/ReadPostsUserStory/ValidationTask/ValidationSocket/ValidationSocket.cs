namespace Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

public class ValidationSocket(ValidationSocket.IValidationPlugin plugin) : IValidationSocket
{
    public class Design
    {
        [Fact]
        public async void Path_Without_Diversion()
        {
            var unit = new ValidationSocket(mockValidationPlugin.Mock);
            request.UseValidRequest();

            var response = await unit.Validate(request.Mock, token);

            response.Should().NotBeNullOrEmpty();
            response.Should().OnlyContain(result => mockValidationPlugin.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
            await mockValidationPlugin.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
        }

        private readonly IValidationPlugin.MockBuilder mockValidationPlugin = new();
        private readonly Request.MockBuilder request = new();
        CancellationToken token = CancellationToken.None;

    }

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

            public MockBuilder() => MockFailedValidation();
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
}

