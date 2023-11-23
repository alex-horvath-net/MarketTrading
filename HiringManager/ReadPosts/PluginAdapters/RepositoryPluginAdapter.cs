using Core.Business.DomainModel;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Blogger.ReadPosts.PluginAdapters;

public class RepositoryPluginAdapter(PluginAdapters.IRepositoryPlugin repositoryPlugin) : Business.IRepositoryPluginAdapter
{
    public async Task<List<Post>> Read(Business.IFeature.Request request, CancellationToken token)
    {
        var adapter = await repositoryPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Core.Business.DomainModel.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }

    public class Read_Specification
    {
        [Fact]
        public async void Path_Without_Diversion()
        {
            var unit = new PluginAdapters.RepositoryPluginAdapter(repositoryPlugin.Mock);
            var response = await unit.Read(feature.Request, feature.Token);
            response.Should().NotBeNullOrEmpty();
            response.Should().OnlyContain(result => repositoryPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
            await repositoryPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
        }

        private readonly PluginAdapters.IRepositoryPlugin.MockBuilder repositoryPlugin = new();
        private readonly Business.IFeature.MockBuilder feature = new();

    }
}

