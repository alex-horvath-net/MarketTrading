using Core.Enterprise;
using Core.Enterprise.UserStory;
using FluentAssertions;
using NSubstitute;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;
using Xunit;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;

public class ValidationTask(IValidationSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        return response.Validations.Any(x => !x.IsSuccess);
    }

    public class Design
    {
        [Fact]
        public void ItHasSockets()
        {
            var providePostsTask = new ValidationTask(validationSocket.Mock);

            providePostsTask.Should().NotBeNull();
            providePostsTask.Should().BeAssignableTo<IUserTask<Request, Response>>();
        }

        [Fact]
        public async void ItCanPopulatePostsOfResponse()
        {
            response.WillHaveValidRequest();
            validationSocket.WillValid();
            var validationTask = new ValidationTask(validationSocket.Mock);

            var terminated = await validationTask.Run(response.Mock, token);

            terminated.Should().BeFalse();
            response.Mock.Validations.Should().HaveCount(0);
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

        public MockBuilder WillValid()
        {
            Mock.Validate(default, default)
                .ReturnsForAnyArgs(new List<ValidationResult>()
                {

                });
            return this;
        }
    }
}