using Blogger.ReadPosts.Business;
using Core.Business.DomainModel;

namespace Tests.Blogger.ReadPosts.Business
{
    internal class RepositoryPluginAdapter_MockBuilder
    {
        public readonly IRepositoryPluginAdapter Mock = Substitute.For<IRepositoryPluginAdapter>();

        public RepositoryPluginAdapter_MockBuilder() => MockReadPosts();

        public RepositoryPluginAdapter_MockBuilder MockReadPosts()
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
