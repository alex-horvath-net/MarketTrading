using Core.Enterprise.UserStory;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;

public class ValidationTask(IValidationSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        var hasValidationIssue = response.Validations.Any(x => !x.IsSuccess);
        return hasValidationIssue;
    }

    public class Design
    {
        [Fact]
        public void ItHas_Sockets()
        {
            var validationTask = new ValidationTask(validationSocket.Mock);

            validationTask.Should().NotBeNull();
            validationTask.Should().BeAssignableTo<IUserTask<Request, Response>>();
        }

        [Fact]
        public async void ItCan_ValidateValidRequest()
        { 
            validationSocket.Pass();
            var validationTask = new ValidationTask(validationSocket.Mock);

            var terminated = await validationTask.Run(response.Mock, token);

            terminated.Should().BeFalse();
            response.Mock.Validations.Should().OnlyContain(x=>x.IsSuccess);
            await validationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
        }

        [Fact]
        public async void ItCan_ValidateInValidRequest()
        {
            validationSocket.Fail();
            var validationTask = new ValidationTask(validationSocket.Mock);

            var terminated = await validationTask.Run(response.Mock, token);

            terminated.Should().BeTrue();
            response.Mock.Validations.Should().Contain(x => !x.IsSuccess);
            await validationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
        }

        private readonly IValidationSocket.MockBuilder validationSocket = new();
        private readonly Response.MockMuilder response = new();
        private readonly CancellationToken token = CancellationToken.None;
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
                    ValidationResult.Success(),
                    ValidationResult.Success()
                });
            return this;
        }

        public MockBuilder Fail()
        {
            Mock.Validate(default, default)
                .ReturnsForAnyArgs(new List<ValidationResult>()
                {
                    ValidationResult.Success(),
                    ValidationResult.Failed("TestErrorCode", "TestErrorMessage")
                });
            return this;
        }
    }
}