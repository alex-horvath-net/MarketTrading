using Core.Application.Sockets.DataModel;
using Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;

namespace Design.Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;

public class IDataAccessPlugin_MockBuilder
{
    public readonly IDataAccessPlugin Mock = Substitute.For<IDataAccessPlugin>();

    public List<Post> Results { get; internal set; }

    public IDataAccessPlugin_MockBuilder MockRead()
    {
        Results =
        [
            new() { Title = "Title", Content = "Content" }
        ];
        Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}
