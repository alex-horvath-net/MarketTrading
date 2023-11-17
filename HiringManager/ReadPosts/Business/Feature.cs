using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Blogger.ReadPosts.Business;

public class Feature
{
    public class MockBuilder
    {
        public Request Request;
        public CancellationToken Token;

        public MockBuilder DefaultRequest()
        {
            Request = new Request("Title", "Content");
            return this;
        }

        public MockBuilder DefaultToken()
        {
            Token = CancellationToken.None;
            return this;
        }
    }
    public class Specify_Run
    {
        [Fact]
        public async void Response_Should_Be_Not_Null()
        {
            var unit = new Feature(validator.Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);
            response.Should().NotBeNull();
        }

        [Fact]
        public async void Response_Shoud_Contain_The_Request()
        {
            var unit = new Feature(validator.Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);
            response.Request.Should().Be(inputs.Request);
        }

        [Fact]
        public async void Response_Should_Not_Contain_Posts_If_The_Request_InValid()
        {
            var unit = new Feature(validator.Failed().Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);
            response.Posts.Should().BeNull();
        }

        [Fact]
        public async void Response_Should_Contain_Posts_If_The_Request_Valid()
        {
            var unit = new Feature(validator.Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);
            response.Posts.Should().NotBeNull();
        }

        [Fact]
        public async void Response_Should_Contain_Only_Matching_Posts()
        {
            var unit = new Feature(validator.Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);
            response.Posts.Should().OnlyContain(post =>
                post.Title.Contains(inputs.Request.Title) ||
                post.Content.Contains(inputs.Request.Content));
        }

        [Fact]
        public async void Read_If_Valid()
        {
            var unit = new Feature(validator.Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);

            response.ValidationResult.IsSuccess.Should().BeTrue();
            response.Posts.Should().NotBeNull();
            await validator.Mock.Received(1).Validate(inputs.Request, inputs.Token);
            await repository.Mock.Received(1).Read(inputs.Request, inputs.Token);
        }

        [Fact]
        public async void Dont_Read_If_InValid()
        {
            var unit = new Feature(validator.Failed().Mock, repository.Mock);
            var response = await unit.Run(inputs.Request, inputs.Token);

            await validator.Mock.Received(1).Validate(inputs.Request, inputs.Token);
            await repository.Mock.Received(0).Read(inputs.Request, inputs.Token);
        }

        public Specify_Run()
        {
            validator = new IValidator.MockBuilder().Success();
            repository = new IRepository.MockBuilder().Read();
            inputs = new Feature.MockBuilder().DefaultRequest().DefaultToken();
        }

        private IValidator.MockBuilder validator;
        private IRepository.MockBuilder repository;
        private Feature.MockBuilder inputs;
    }

    public async Task<Response> Run(Request request, CancellationToken token)
    {
        var response = new Response(request);
        response.ValidationResult = await validator.Validate(request, token);
        if (response.ValidationResult.IsSuccess)
        {
            response.Posts = await repository.Read(request, token);
        }
        return response;
    }

    public Feature(IValidator validator, IRepository repository)
    {
        this.validator = validator;
        this.repository = repository;
    }

    private readonly IValidator validator;
    private readonly IRepository repository;
}