using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Blogger.ReadPosts.Business;

public class Feature
{
    public class Specify
    {
        [Fact]
        public void How_to_create_it()
        {
            var validator = IValidator.Mock.Simple();
            var repository = IRepository.Mock.Simple();
            var unit = new Feature(validator, repository);
            unit.Should().NotBeNull();
        }


        [Fact]
        public async void Which_unit_should_validate_the_request()
        {
            var validator = IValidator.Mock.Simple();
            var repository = IRepository.Mock.Simple();
            var unit = new Feature(validator, repository);
            var request = new Request("Title", "Content");
            var token = CancellationToken.None;
            await unit.Run(request, token);
            await validator.Received(1).Validate(request, token);
        }

        [Fact]
        public async void Which_unit_should_find_the_jbob_roles()
        {
            var validator = IValidator.Mock.Simple();
            var repository = IRepository.Mock.Simple();
            var unit = new Feature(validator, repository);
            var request = new Request("Title", "Content");
            var token = CancellationToken.None;
            await unit.Run(request, token);
            await repository.Received(1).Read(request, token);
        }

        [Fact]
        public async void What_should_be_the_Response()
        {
            var validator = IValidator.Mock.Simple();
            var repository = IRepository.Mock.Simple();
            var unit = new Feature(validator, repository);
            var request = new Request("Title", "Content");
            var token = CancellationToken.None;
            var response = await unit.Run(request, token);
            response.Should().NotBeNull(null);
            response.Posts.Should().NotBeNullOrEmpty();
        }
    }

    public async Task<Response> Run(Request request, CancellationToken token)
    {
        var response = new Response();
        var result = await validator.Validate(request, token);
        if (result.IsSuccess)
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