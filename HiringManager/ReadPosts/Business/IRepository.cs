using NSubstitute;
using Shared.Business.DomainModel;

namespace Blogger.ReadPosts.Business;

public interface IRepository
{
    Task<List<Post>> Read(Request request, CancellationToken token);

    public static class Mock
    {
        public static IRepository Simple()
        {
            var mock = Substitute.For<IRepository>();
            var response = new List<Post>
            {
                new Post{ Title= "Title1", Content= "Content1"},
                new Post{ Title= "Title2", Content= "Content2"},
                new Post{ Title= "Title3", Content= "Content3"}
            };
            mock.Read(default, default).ReturnsForAnyArgs(response);
            return mock;
        }
    }
}