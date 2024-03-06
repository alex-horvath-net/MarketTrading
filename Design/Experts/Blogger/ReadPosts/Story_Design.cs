//using Common.Business.Model;
//using Core;
//using Core.Business;
//using Core.Business.Model;

//namespace Experts.Blogger.ReadPosts;

//public class Story_Design(ITestOutputHelper output) : UnitDesign<UserStory<UserStoryRequest, Response>>(output) {
//    private void Create() => Unit = new UserStory<UserStoryRequest, Response>(workSteps, presenter, logger, time);

//    private async Task Use() => response = await Unit.Run(request, token);

//    [Fact]
//    public void ItHas_Dependencies() {
//        Create();

//        Unit.Should().NotBeNull();
//        Unit.Should().BeAssignableTo<UserStory<UserStoryRequest, Response>>();
//    }

//    [Fact]
//    public async void ItCan_ValidateValidRequest() {
//        settings.Enabled();
//        time.Freeze_2023_01_01();
//        validator.MockPass();
//        Create();
//        request.MockValidRequest();

//        await Use();

//        response.MetaData.Stoped.Should().NotBeNull();
//        response.MetaData.RequestIssues.Should().NotContain(x => !x.IsSuccess);
//        response.MetaData.RequestIssues.Should().BeEmpty();
//        await validator.ReceivedWithAnyArgs().Validate(default, default);
//    }

//    [Fact]
//    public async void ItCan_ValidateInValidRequest() {
//        settings.Enabled();
//        time.Freeze_2023_01_01();
//        validator.MockFail();
//        Create();
//        request.MockMissingpProperties();

//        await Use();

//        response.MetaData.Stoped.Should().BeNull();
//        response.MetaData.RequestIssues.Should().Contain(x => !x.IsSuccess);
//        await validator.ReceivedWithAnyArgs().Validate(default, default);
//    }


//    [Fact]
//    public async void ItCan_PopulatePosts() {
//        settings.Enabled();
//        time.Freeze_2023_01_01();
//        request.MockValidRequest();
//        repository.CanReceveRead();
//        Create();

//        await Use();

//        await repository.ReceivedRead();
//        response.Posts.Should().NotBeEmpty();
//        response.MetaData.Stoped.Should().NotBeNull();
//    }

//    private readonly ITime time = Substitute.For<ITime>();
//    private readonly ISettings<Settings> settings = Substitute.For<ISettings<Settings>>();
//    private readonly Presenter presenter = Substitute.For<Presenter>();
//    //private readonly IValidator validator = Substitute.For<IValidator>();
//    //private readonly IRepository repository = Substitute.For<IRepository>();
//    private readonly ILog<UserStory<UserStoryRequest,Response>> logger = Substitute.For<ILog<UserStory<UserStoryRequest,Response>>>();
//    private readonly UserStoryRequest request = new UserStoryRequest(default);
//    private readonly IEnumerable<IUserWorkStep<UserStoryRequest, Response>> workSteps = Substitute.For<IEnumerable<IUserWorkStep<UserStoryRequest, Response>>>();
//    private Response response = new Response();
//}

//public class Validation_Design(ITestOutputHelper output) : UnitDesign<Validator>(output) {
//    private void Create() => Unit = new Validator();

//    private async Task Act() => issues = await Unit.Validate(request, token);

//    [Fact]
//    public void ItHas_NoDependecy() {
//        Create();

//        Unit.Should().NotBeNull();
//    }

//    [Fact]
//    public async void ItCan_AllowValidRequest() {
//        Create();
//        request = request.MockValidRequest();

//        await Act();

//        issues.Should().NotBeNull();
//        issues.Should().BeEmpty();
//    }

//    [Fact]
//    public async void ItCan_FindMissingFiltersOfRequest() {
//        Create();
//        request = request.MockMissingpProperties();

//        await Act();

//        issues.Should().NotBeNull();
//        issues.Should().HaveCount(2);
//        issues.Should().ContainSingle(x =>
//            //x.PropertyName == "Title" &&
//            x.ErrorCode == "NotEmptyValidator" &&
//            x.ErrorMessage == "'Title' can not be empty if 'Content' is empty.");
//        //x.Severity == "Error");

//        issues.Should().ContainSingle(x =>
//            //x.PropertyName == "Content" &&
//            x.ErrorCode == "NotEmptyValidator" &&
//            x.ErrorMessage == "'Content' can not be empty if 'Title' is empty.");
//        //x.Severity == "Error");
//    }

//    [Fact]
//    public async void ItCan_FindShortFiltersOfRequest() {
//        Create();
//        request = request.MockTooShortProperties();

//        await Act();

//        issues.Should().NotBeNull();
//        issues.Should().HaveCount(2);
//        issues.Should().ContainSingle(x =>
//            //x.PropertyName == "Title" &&
//            x.ErrorCode == "MinimumLengthValidator" &&
//            x.ErrorMessage == "The length of 'Title' must be at least 3 characters. You entered 2 characters."
//            //x.Severity == "Error"
//            );

//        issues.Should().ContainSingle(x =>
//            //x.PropertyName == "Content" &&
//            x.ErrorCode == "MinimumLengthValidator" &&
//            x.ErrorMessage == "The length of 'Content' must be at least 3 characters. You entered 2 characters."
//            //x.Severity == "Error"
//            );
//    }

//    private UserStoryRequest request = new(default);
//    private IEnumerable<Result> issues;
//}

//public static class Extensions {
//    public static ISettings<Settings> Enabled(this ISettings<Settings> settings) {
//        var returnThis = new Settings() { Enabled = true };
//        settings.Value.Returns(returnThis);
//        return settings;
//    }

//    public static ISettings<Settings> Disabled(this ISettings<Settings> settings) {
//        var returnThis = new Settings() { Enabled = false };
//        settings.Value.Returns(returnThis);
//        return settings;
//    }

//    public static ITime Freeze_2023_01_01(this ITime time) {
//        var returnThis = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//        time.UtcNow.Returns(returnThis);
//        return time;
//    }

//    public static IValidator<UserStoryRequest> MockPass(this IValidator<UserStoryRequest> solution) {
//        solution
//            .Validate(default, default)
//            .ReturnsForAnyArgs(new List<Result>() { });
//        return solution;
//    }

//    public static IValidator<UserStoryRequest> MockFail(this IValidator<UserStoryRequest> solution) {
//        solution
//            .Validate(default, default)
//            .ReturnsForAnyArgs(new List<Result>()
//            {
//                new Failed("TestErrorCode", "TestErrorMessage")
//                //new ("TestPropertyName", "TestErrorCode", "TestErrorMessage", "TestSeverity")
//            });
//        return solution;
//    }

//    public static IRepository CanReceveRead(this IRepository solution) {
//        solution
//            .Read(default, default)
//            .ReturnsForAnyArgs(new List<Post> {
//                        new(){ Id= 1, Title= "Title1", Content= "Content1",  CreatedAt= DateTime.UtcNow},
//                        new(){ Id= 2, Title= "Title2", Content= "Content2",  CreatedAt= DateTime.UtcNow},
//                        new(){ Id= 3, Title= "Title3", Content= "Content3",  CreatedAt= DateTime.UtcNow}
//            });
//        return solution;
//    }

//    public static async Task<IRepository> ReceivedRead(this IRepository solution) {
//        await solution.ReceivedWithAnyArgs().Read(default, default);
//        return solution;
//    }


//    public static UserStoryRequest MockValidRequest(this UserStoryRequest request) =>
//        new UserStoryRequest("Content");

//    public static UserStoryRequest MockMissingpProperties(this UserStoryRequest request) =>
//        request = new UserStoryRequest(null);

//    public static UserStoryRequest MockTooShortProperties(this UserStoryRequest request) =>
//        new UserStoryRequest("12");

//    public static Response MockNoPosts(this Response response) {
//        response.MockValidRequest();
//        response.Posts = null;
//        return response;
//    }

//    public static Response MockValidRequest(this Response response) {
//        response.MetaData.UserStoryRequest = new UserStoryRequest(default).MockValidRequest();
//        response.MetaData.Enabled = true;
//        response.MetaData.RequestIssues = null;
//        return response;
//    }

//    public static Response MockNoValidations(this Response response) {
//        response.MockValidRequest();
//        response.MetaData.RequestIssues = null;
//        return response;
//    }
//}
