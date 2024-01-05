using Core.Enterprise;
using Core.Enterprise.UserStory;
using FluentAssertions;
using NSubstitute;
using Users.Blogger.ReadPostsUserStory;
using Xunit;
using Xunit.Abstractions;

namespace Users.Blogger.ReadPostsUserStory.UserTasks.ValidationTask;

public class ValidationTask(IValidationSocket socket) : IUserTask<Request, Response>
{
    public class Design : Design<ValidationTask>
    {
        private void Construct() => unit = new(validationSocket);

        private async Task Run() => terminated = await unit.Run(response, token);

        [Fact]
        public void ItHas_Sockets()
        {
            Construct();

            unit.Should().NotBeNull();
            unit.Should().BeAssignableTo<IUserTask<Request, Response>>();
        }

        [Fact]
        public async void ItCan_ValidateValidRequest()
        {
            mockValidationSocket.Pass();
            Construct();
            mockResponse.HasNoValidations();

            await Run();

            terminated.Should().BeFalse();
            mockResponse.Mock.Validations.Should().NotContain(x => !x.IsSuccess);
            mockResponse.Mock.Validations.Should().BeEmpty();
            await mockValidationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
        }

        [Fact]
        public async void ItCan_ValidateInValidRequest()
        {
            mockValidationSocket.Fail();
            Construct();
            mockResponse.HasNoValidations();

            await Run();

            terminated.Should().BeTrue();
            mockResponse.Mock.Validations.Should().Contain(x => !x.IsSuccess);
            await mockValidationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
        }

        private readonly IValidationSocket.MockBuilder mockValidationSocket = new();
        private IValidationSocket validationSocket => mockValidationSocket.Mock;
        private readonly Response.MockMuilder mockResponse = new();
        private Response response => mockResponse.Mock;
        private readonly CancellationTokenBuilder tokenBuilder = new();
        private CancellationToken token => tokenBuilder.Token;
        private ValidationTask unit;
        private bool terminated;


        public Design(ITestOutputHelper output) : base(output)
        {
        }
    }

    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        var hasValidationIssue = response.Validations.Any(x => !x.IsSuccess);
        return hasValidationIssue;
    }
}

public interface IValidationSocket
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);

    public class MockBuilder
    {
        public IValidationSocket Mock { get; } = Substitute.For<IValidationSocket>();

        public MockBuilder Pass()
        {
            Mock.Validate(default, default)
                .ReturnsForAnyArgs(new List<ValidationResult>()
                {
                });
            return this;
        }

        public MockBuilder Fail()
        {
            Mock.Validate(default, default)
                .ReturnsForAnyArgs(new List<ValidationResult>()
                {
                    ValidationResult.Failed("TestErrorCode", "TestErrorMessage")
                });
            return this;
        }
    }
}