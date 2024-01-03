using Castle.Components.DictionaryAdapter.Xml;
using Core.Application.UserStory.DomainModel;
using Core.Enterprise;
using Core.Enterprise.UserStory;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;

public class ReadPostsTask(IDataAccessSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Posts = await socket.Read(response.Request, token);
        return false;
    }

    public class Design
    {
        [Fact]
        public void ItHasSockets()
        {
            var providePostsTask = new ReadPostsTask(dataAccessSocket.Mock);

            providePostsTask.Should().NotBeNull();
            providePostsTask.Should().BeAssignableTo<IUserTask<Request, Response>>();
        }

        [Fact]
        public async void ItCanPopulatePostsOfResponse()
        {
            response.WillHaveNoPosts();
            dataAccessSocket.WillProvide3Posts();
            var providePostsTask = new ReadPostsTask(dataAccessSocket.Mock);

            var terminated = await providePostsTask.Run(response.Mock, token);

            terminated.Should().BeFalse();
            response.Mock.Posts.Should().HaveCount(3);
            await dataAccessSocket.Mock.ReceivedWithAnyArgs().Read(default, default);
        }

        private readonly IDataAccessSocket.MockBuilder dataAccessSocket = new();
        private readonly Response.MockMuilder response = new();
        private readonly CancellationToken token = CancellationToken.None;
    }
}



public interface IDataAccessSocket
{
    Task<List<Post>> Read(Request request, CancellationToken token);

    public class MockBuilder
    {
        public IDataAccessSocket Mock { get; } = Substitute.For<IDataAccessSocket>();

        public MockBuilder WillProvide3Posts()
        {
            Mock.Read(default, default)
                .ReturnsForAnyArgs(new List<Post>()
                {
                    new Post() { Id = 1, Title = "Post 1", Content = "Content 1" },
                    new Post() { Id = 2, Title = "Post 2", Content = "Content 2" },
                    new Post() { Id = 3, Title = "Post 3", Content = "Content 3" }
                }.ToTask());
            return this;
        }
    }
}
