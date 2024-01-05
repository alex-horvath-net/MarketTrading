
using Core.Application.UserStory.DomainModel;
using Core.Enterprise;
using FluentAssertions;
using NSubstitute;
using Users.Blogger.UserStories.ReadPostsUserStory;
using Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask;
using Xunit;
using Xunit.Abstractions;

namespace Users.Blogger.UserStories.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket;

public class DataAccessSocket(DataAccessSocket.IDataAccessPlugin plugin) : ReadPostsTask.IDataAccessSocket
{
    public class Design : Design<DataAccessSocket>
    {
        private void Construct() => Unit = new(dataAccessPlugin);

        private async Task Run() => response = await Unit.Read(request, Token);

        [Fact]
        public void ItRequires_Plugins()
        {
            Construct();

            Unit.Should().NotBeNull();
            Unit.Should().BeAssignableTo<ReadPostsTask.IDataAccessSocket>();
        }

        [Fact]
        public async void Path_Without_Diversion()
        {
            Construct();

            mockRequest.UseInvaliedRequestWithMissingFilters();

            await Run();

            response.Should().NotBeNullOrEmpty();
            response.Should().OnlyContain(result => mockDataAccessPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
            await mockDataAccessPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
        }

        private readonly IDataAccessPlugin.MockBuilder mockDataAccessPlugin = new();
        private IDataAccessPlugin dataAccessPlugin => mockDataAccessPlugin.Mock;
        private readonly Request.MockBuilder mockRequest = new();
        private Request request => mockRequest.Mock;
        private List<Post> response;

        public Design(ITestOutputHelper output) : base(output)
        {
        }
    }

    public async Task<List<Post>> Read(Request request, CancellationToken token)
    {
        var socketDataModel = await plugin.Read(request.Title, request.Content, token);
        var userStoryDomainModel = socketDataModel.Select(x => new Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return userStoryDomainModel;
    }

    public interface IDataAccessPlugin
    {
        Task<List<Core.Application.Sockets.DataModel.Post>> Read(string title, string content, CancellationToken token);

        public class MockBuilder
        {
            public readonly IDataAccessPlugin Mock = Substitute.For<IDataAccessPlugin>();
            public List<Core.Application.Sockets.DataModel.Post> Results { get; internal set; }

            public MockBuilder() => MockRead();

            public MockBuilder MockRead()
            {
                Results = new List<Core.Application.Sockets.DataModel.Post>
                {
                    new() {  Title= "Title", Content="Content"}
                };
                Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
                return this;
            }
        }
    }
}

