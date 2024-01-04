using Core.Application.UserStory.DomainModel;
using Core.Enterprise.UserStory;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;

public class ReadPostsTask(ReadPostsTask.IDataAccessSocket socket) :
    IUserTask<Request, Response>
{
    public class Design
    {
        private void Constructor() => unit = new ReadPostsTask(dataAccessSocket.Mock);
        
        private async Task Beahaviour() => terminated = await unit.Run(response.Mock, token);

        [Fact]
        public void ItRequires_Sockets()
        {
            Constructor();

            unit.Should().NotBeNull();
            unit.Should().BeAssignableTo<IUserTask<Request, Response>>();
        }

        [Fact]
        public async void ItCan_PopulateResponseWithPosts()
        {
            response.HasNoPosts();
            dataAccessSocket.ProvidesPosts();
            Constructor();

            await Beahaviour();

            terminated.Should().BeFalse();
            response.Mock.Posts.Should().NotBeEmpty();
            await dataAccessSocket.Mock.ReceivedWithAnyArgs().Read(default, default);
        }

        private readonly IDataAccessSocket.MockBuilder dataAccessSocket = new();
        private readonly Response.MockMuilder response = new();
        private readonly CancellationToken token = CancellationToken.None;
        private ReadPostsTask unit;
        private bool terminated;
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
