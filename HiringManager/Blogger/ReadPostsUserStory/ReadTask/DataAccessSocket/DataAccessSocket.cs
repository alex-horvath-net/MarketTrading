namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

public class DataAccessSocket(DataAccessSocket.IDataAccessPlugin plugin) : ReadPostsTask.IDataAccessSocket
{
    public async Task<List<DomainModel.Post>> Read(Request request, CancellationToken token)
    {
        var dataModel = await plugin.Read(request.Title, request.Content, token);
        var userStoryDomainModel = dataModel.Select(x => new DomainModel.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return userStoryDomainModel;
    }

    public interface IDataAccessPlugin
    {
        Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token);

        public class MockBuilder
        {
            public readonly IDataAccessPlugin Mock = Substitute.For<IDataAccessPlugin>();
            public List<DataModel.Post> Results { get; internal set; }

            public MockBuilder() => MockRead();

            public MockBuilder MockRead()
            {
                Results =
                [
                    new() { Title = "Title", Content = "Content" }
                ];
                Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
                return this;
            }
        }
    }

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
        private List<DomainModel.Post> response;

        public Design(ITestOutputHelper output) : base(output)
        {
        }
    }
}
