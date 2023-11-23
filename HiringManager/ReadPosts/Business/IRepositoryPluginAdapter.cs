using Core.Business.DomainModel;
using NSubstitute;

namespace Blogger.ReadPosts.Business;

public interface IRepositoryPluginAdapter
{
    Task<List<Post>> Read(IFeature.Request request, CancellationToken cancellation);

    public class MockBuilder
    {
        public readonly IRepositoryPluginAdapter Mock = Substitute.For<IRepositoryPluginAdapter>();

        public MockBuilder()=> MockReadPosts();

        public MockBuilder MockReadPosts()
        {
            var response = new List<Post>
            {                                                              
                new Core.Business.DomainModel.Post{ Title= "Title1", Content= "Content1"},
                new Core.Business.DomainModel.Post{ Title= "Title2", Content= "Content2"},
                new Core.Business.DomainModel.Post{ Title= "Title3", Content= "Content3"}
            };
            Mock.Read(default, default).ReturnsForAnyArgs(response);
            return this;
        }

    }
}