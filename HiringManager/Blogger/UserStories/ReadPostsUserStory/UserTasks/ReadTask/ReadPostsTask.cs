using Core.Application.UserStory.DomainModel;
using Core.Enterprise;
using Core.Enterprise.UserStory;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask;

public class ReadPostsTask(ReadPostsTask.IDataAccessSocket socket) :
    IUserTask<Request, Response>
{
    public class Design : Design<ReadPostsTask>
    {
        private void Construct() => Unit = new(dataAccessSocket);

        private async Task Run() => terminated = await Unit.Run(response, Token);

        [Fact]
        public void ItRequires_Sockets()
        {
            Construct();

            Unit.Should().NotBeNull();
            Unit.Should().BeAssignableTo<IUserTask<Request, Response>>();
        }

        [Fact]
        public async void ItCan_PopulateResponseWithPosts()
        {
            mockResponse.HasNoPosts();
            mockDataAccessSocket.ProvidesPosts();
            Construct();

            await Run();

            terminated.Should().BeFalse();
            mockResponse.Mock.Posts.Should().NotBeEmpty();
            await mockDataAccessSocket.Mock.ReceivedWithAnyArgs().Read(default, default);
        }

        private readonly IDataAccessSocket.MockBuilder mockDataAccessSocket = new();
        private IDataAccessSocket dataAccessSocket => mockDataAccessSocket.Mock;
        private readonly Response.MockMuilder mockResponse = new();
        private Response response => mockResponse.Mock;
        private bool terminated;

        public Design(ITestOutputHelper output) : base(output)
        {
        }
    }

    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Posts = await socket.Read(response.Request, token);
        return false;
    }

    public interface IDataAccessSocket
    {
        Task<List<Post>> Read(Request request, CancellationToken token);

        public class MockBuilder
        {
            public IDataAccessSocket Mock { get; } = Substitute.For<IDataAccessSocket>();

            public MockBuilder ProvidesPosts()
            {
                Mock.Read(default, default)
                    .ReturnsForAnyArgs(
                    [
                        new() { Id = 1, Title = "Post 1", Content = "Content 1" },
                        new() { Id = 2, Title = "Post 2", Content = "Content 2" },
                        new() { Id = 3, Title = "Post 3", Content = "Content 3" }
                    ]);
                return this;
            }
        }
    }
}
