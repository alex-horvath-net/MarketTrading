using Common.Business.Model;
using Core;
using Core.Business;

namespace Experts.Blogger.ReadPosts;

public class Story_Design(ITestOutputHelper output) : Design<Story>(output) {
    private void Create() => Unit = new Story(repository, validator, logger);

    private async Task Act() => response = await Unit.Run(request, token);

    [Fact]
    public void ItHas_Dependencies() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<StoryCore<Request, Response, Story>>();
    }

    [Fact]
    public async void ItCan_ValidateValidRequest() {
        validator.MockPass();
        Create();
        request.MockValidRequest();

        await Act();

        response.CompletedAt.Should().NotBeNull();
        response.Issues.Should().NotContain(x => !x.IsSuccess);
        response.Issues.Should().BeEmpty();
        await validator.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest() {
        validator.MockFail();
        Create();
        request.MockMissingpProperties();

        await Act();

        response.CompletedAt.Should().BeNull();
        response.Issues.Should().Contain(x => !x.IsSuccess);
        await validator.ReceivedWithAnyArgs().Validate(default, default);
    }


    [Fact]
    public async void ItCan_PopulatePosts() {
        request.MockValidRequest();
        repository.CanReceveRead();
        Create();

        await Act();

        await repository.ReceivedRead();
        response.Posts.Should().NotBeEmpty();
        response.CompletedAt.Should().NotBeNull();
    }

    private readonly IValidator validator = Substitute.For<IValidator>();
    private readonly IRepository repository = Substitute.For<IRepository>();
    private readonly ILogger<Story> logger = Substitute.For<ILogger<Story>>();
    private readonly Request request = new Request(default, default);
    private Response response = new Response();
}

public class Validation_Design(ITestOutputHelper output) : Design<Validation>(output) {
    private void Create() => Unit = new Validation();

    private async Task Act() => issues = await Unit.Validate(request, token);

    [Fact]
    public void ItHas_NoDependecy() {
        Create();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public async void ItCan_AllowValidRequest() {
        Create();
        request = request.MockValidRequest();

        await Act();

        issues.Should().NotBeNull();
        issues.Should().BeEmpty();
    }

    [Fact]
    public async void ItCan_FindMissingFiltersOfRequest() {
        Create();
        request = request.MockMissingpProperties();

        await Act();

        issues.Should().NotBeNull();
        issues.Should().HaveCount(2);
        issues.Should().ContainSingle(x =>
            //x.PropertyName == "Title" &&
            x.ErrorCode == "NotEmptyValidator" &&
            x.ErrorMessage == "'Title' can not be empty if 'Content' is empty.");
        //x.Severity == "Error");

        issues.Should().ContainSingle(x =>
            //x.PropertyName == "Content" &&
            x.ErrorCode == "NotEmptyValidator" &&
            x.ErrorMessage == "'Content' can not be empty if 'Title' is empty.");
        //x.Severity == "Error");
    }

    [Fact]
    public async void ItCan_FindShortFiltersOfRequest() {
        Create();
        request = request.MockTooShortProperties();

        await Act();

        issues.Should().NotBeNull();
        issues.Should().HaveCount(2);
        issues.Should().ContainSingle(x =>
            //x.PropertyName == "Title" &&
            x.ErrorCode == "MinimumLengthValidator" &&
            x.ErrorMessage == "The length of 'Title' must be at least 3 characters. You entered 2 characters."
            //x.Severity == "Error"
            );

        issues.Should().ContainSingle(x =>
            //x.PropertyName == "Content" &&
            x.ErrorCode == "MinimumLengthValidator" &&
            x.ErrorMessage == "The length of 'Content' must be at least 3 characters. You entered 2 characters."
            //x.Severity == "Error"
            );
    }

    private Request request = new(default, default);
    private IEnumerable<Result> issues;
}

public static class Extensions {
    public static IValidator<Request> MockPass(this IValidator<Request> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<Result>() { });
        return solution;
    }

    public static IValidator<Request> MockFail(this IValidator<Request> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<Result>()
            {
                new Failed("TestErrorCode", "TestErrorMessage")
                //new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
            });
        return solution;
    }

    public static IRepository CanReceveRead(this IRepository solution) {
        solution
            .Read(default, default)
            .ReturnsForAnyArgs(new List<Post> {
                        new(){ Id= 1, Title= "Title1", Content= "Content1",  CreatedAt= DateTime.UtcNow},
                        new(){ Id= 2, Title= "Title2", Content= "Content2",  CreatedAt= DateTime.UtcNow},
                        new(){ Id= 3, Title= "Title3", Content= "Content3",  CreatedAt= DateTime.UtcNow}
            });
        return solution;
    }

    public static async Task<IRepository> ReceivedRead(this IRepository solution) {
        await solution.ReceivedWithAnyArgs().Read(default, default);
        return solution;
    }


    public static Request MockValidRequest(this Request request) =>
        new Request("Title", "Content");

    public static Request MockMissingpProperties(this Request request) =>
        request = new Request(null, null);

    public static Request MockTooShortProperties(this Request request) =>
        new Request("12", "21");

    public static Response MockNoPosts(this Response response) {
        response.MockValidRequest();
        response.Posts = null;
        return response;
    }

    public static Response MockValidRequest(this Response response) {
        response.Request = new Request(default, default).MockValidRequest();
        response.Enabled = true;
        response.Issues = null;
        return response;
    }

    public static Response MockNoValidations(this Response response) {
        response.MockValidRequest();
        response.Issues = null;
        return response;
    }
}
