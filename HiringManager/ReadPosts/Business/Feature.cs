using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Blogger.ReadPosts.Business;

public class Feature(
    Business.IValidatorPluginAdapter validator, 
    Business.IRepositoryPluginAdapter repository) : Business.IFeature
{
    public async Task<IFeature.Response> Run(IFeature.Request request, CancellationToken cancellation)
    {
        var response = new IFeature.Response(request);
        response.ValidationResults = await validator.Validate(request, cancellation);
        if (response.ValidationResults.All(x => x.IsSuccess)) response.Posts = await repository.Read(request, cancellation);
        return response;
    }

    public class Run_Specification
    {
        [Fact]
        public async void Invalid_Request()
        {
            validatorAdapter.MockFailedValidation();
           
            var unit = new Business.Feature(validatorAdapter.Mock, repositoryAdapter.Mock);
            var response = await unit.Run(feature.Request, feature.Token);

            response.Should().NotBeNull();
            response.Request.Should().Be(feature.Request);
            response.ValidationResults.Should().NotBeNull();
            await validatorAdapter.Mock.Received(1).Validate(feature.Request, feature.Token);
            response.Posts.Should().BeNull();
            await repositoryAdapter.Mock.Received(0).Read(feature.Request, feature.Token);
        }

        [Fact]
        public async void Valid_Request()
        {
            var unit = new Business.Feature(validatorAdapter.Mock, repositoryAdapter.Mock);
            var response = await unit.Run(feature.Request, feature.Token);

            response.Should().NotBeNull();
            response.Request.Should().Be(feature.Request);
            response.ValidationResults.Should().NotBeNull();
            await validatorAdapter.Mock.Received(1).Validate(feature.Request, feature.Token);
            response.Posts.Should().NotBeNull();
            response.Posts.Should().OnlyContain(post => post.Title.Contains(feature.Request.Title) || post.Content.Contains(feature.Request.Content));
            await repositoryAdapter.Mock.Received(1).Read(feature.Request, feature.Token);
        }

        private readonly Business.IValidatorPluginAdapter.MockBuilder validatorAdapter = new();
        private readonly Business.IRepositoryPluginAdapter.MockBuilder repositoryAdapter = new();
        private readonly Business.IFeature.MockBuilder feature = new();
    }
}


