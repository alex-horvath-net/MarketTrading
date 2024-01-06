using Users.Blogger.ReadPostsUserStory.ReadUserTask;

namespace Design.Users.Blogger.ReadPostsUserStory.ReadUserTask;

public class IDataAccessSocket_MockBuilder
{
    public IDataAccessSocket Mock { get; } = Substitute.For<IDataAccessSocket>();

    public IDataAccessSocket_MockBuilder ProvidesPosts()
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

