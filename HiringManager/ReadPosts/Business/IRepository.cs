using NSubstitute;
using Shared.Business.DomainModel;

namespace Blogger.ReadPosts.Business;

public interface IRepository
{
    Task<List<Post>> Read(Request request, CancellationToken token);

    public class MockBuilder
    {
        public readonly IRepository Mock = Substitute.For<IRepository>();

        public MockBuilder Read()
        {
            var response = new List<Post>
            {                                                              
                new Post{ Title= "Title1", Content= "Content1"},
                new Post{ Title= "Title2", Content= "Content2"},
                new Post{ Title= "Title3", Content= "Content3"}
            };
            Mock.Read(default, default).ReturnsForAnyArgs(response);
            return this;
        }

    }
}