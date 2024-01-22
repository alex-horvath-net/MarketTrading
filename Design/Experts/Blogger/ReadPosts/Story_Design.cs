using Common.Business.Model;
using Core;
using Core.Business;

namespace Experts.Blogger.ReadPosts;

public class Story_Design(ITestOutputHelper output) : Design<Story>(output) {
    private void Create() => Unit = new Story(validation, repository);

    private async Task Act() => response = await Unit.Run(request, token);

    [Fact]
    public void ItHas_Dependencies() {
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<Story<Story.Request, Story.Response>>();
    }

    [Fact]
    public async void ItCan_ValidateValidRequest() {
        validation.MockPass();
        Create();
        request.MockValidRequest();

        await Act();

        response.Terminated.Should().BeFalse();
        response.ValidationResults.Should().NotContain(x => !x.IsSuccess);
        response.ValidationResults.Should().BeEmpty();
        await validation.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest() {
        validation.MockFail();
        Create();
        request.MockMissingpProperties();

        await Act();

        response.Terminated.Should().BeTrue();
        response.ValidationResults.Should().Contain(x => !x.IsSuccess);
        await validation.ReceivedWithAnyArgs().Validate(default, default);
    }


    [Fact]
    public async void ItCan_PopulatePosts() {
        request.MockValidRequest();
        repository.CanReceveRead();
        Create();

        await Act();

        await repository.ReceivedRead();
        response.Posts.Should().NotBeEmpty();
        response.Terminated.Should().BeFalse();
    }

    private readonly IValidation<Story.Request> validation = Substitute.For<IValidation<Story.Request>>();
    private readonly Story.IRepository repository = Substitute.For<Story.IRepository>();
    private readonly Story.Request request = Story.Request.Empty();
    private Story.Response response = Story.Response.Empty();
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

    private Story.Request request = Story.Request.Empty();
    private IEnumerable<ValidationResult> issues;
}

public static class Extensions {
    public static IValidation<Story.Request> MockPass(this IValidation<Story.Request> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>() { });
        return solution;
    }

    public static IValidation<Story.Request> MockFail(this IValidation<Story.Request> solution) {
        solution
            .Validate(default, default)
            .ReturnsForAnyArgs(new List<ValidationResult>()
            {
                ValidationResult.Failed("TestErrorCode", "TestErrorMessage")
                //new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
            });
        return solution;
    }

    public static Story.IRepository CanReceveRead(this Story.IRepository solution) {
        solution
            .Read(default, default)
            .ReturnsForAnyArgs(new List<Post> {
                        new(){ Id= 1, Title= "Title1", Content= "Content1",  CreatedAt= DateTime.UtcNow},
                        new(){ Id= 2, Title= "Title2", Content= "Content2",  CreatedAt= DateTime.UtcNow},
                        new(){ Id= 3, Title= "Title3", Content= "Content3",  CreatedAt= DateTime.UtcNow}
            });
        return solution;
    }

    public static async Task<Story.IRepository> ReceivedRead(this Story.IRepository solution) {
        await solution.ReceivedWithAnyArgs().Read(default, default);
        return solution;
    }


    public static Story.Request MockValidRequest(this Story.Request request) =>
        new Story.Request("Title", "Content");

    public static Story.Request MockMissingpProperties(this Story.Request request) =>
        request = new Story.Request(null, null);

    public static Story.Request MockTooShortProperties(this Story.Request request) =>
        new Story.Request("12", "21");

    public static Story.Response MockNoPosts(this Story.Response response) {
        response.MockValidRequest();
        response.Posts = null;
        return response;
    }

    public static Story.Response MockValidRequest(this Story.Response response) {
        response.Request = Story.Request.Empty().MockValidRequest();
        response.FeatureEnabled = true;
        response.ValidationResults = null;
        return response;
    }

    public static Story.Response MockNoValidations(this Story.Response response) {
        response.MockValidRequest();
        response.ValidationResults = null;
        return response;
    }
}
