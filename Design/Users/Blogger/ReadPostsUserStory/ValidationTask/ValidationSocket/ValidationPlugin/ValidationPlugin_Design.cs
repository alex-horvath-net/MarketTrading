using Core.Enterprise.Sockets.Validation;
using Design.Core.Enterprise;
using Users.Blogger.ReadPostsUserStory;
using SUT = Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationPlugin.ValidationPlugin;

namespace Design.Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationPlugin;

public class ValidationPlugin_Design : Design<SUT> 
{
    private void Construct() => Unit = new SUT();

    private async Task Validate() => issues = await Unit.Validate(request, Token);

    [Fact]
    public void ItHas_NoDependecy()
    {
        Construct();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public async void ItCan_AllowValidRequest()
    {
        Construct();
        mockRequest.UseValidRequest();

        await Validate();

        issues.Should().NotBeNull();
        issues.Should().BeEmpty();
    }

    [Fact]
    public async void ItCan_FindMissingFiltersOfRequest()
    {
        Construct();
        mockRequest.UseInvaliedRequestWithMissingFilters();

        await Validate();

        issues.Should().NotBeNull();
        issues.Should().HaveCount(2);
        issues.Should().ContainSingle(x =>
            x.PropertyName == "Title" &&
            x.ErrorCode == "NotEmptyValidator" &&
            x.ErrorMessage == "'Title' can not be empty if 'Content' is empty." &&
            x.Severity == "Error");

        issues.Should().ContainSingle(x =>
            x.PropertyName == "Content" &&
            x.ErrorCode == "NotEmptyValidator" &&
            x.ErrorMessage == "'Content' can not be empty if 'Title' is empty." &&
            x.Severity == "Error");
    } 

    [Fact]
    public async void ItCan_FindShortFiltersOfRequest()
    {
        Construct();
        mockRequest.UseInvaliedRequestWithShortFilters();

        await Validate();

        issues.Should().NotBeNull();
        issues.Should().HaveCount(2);
        issues.Should().ContainSingle(x =>
            x.PropertyName == "Title" &&
            x.ErrorCode == "MinimumLengthValidator" &&
            x.ErrorMessage == "The length of 'Title' must be at least 3 characters. You entered 2 characters." &&
            x.Severity == "Error");

        issues.Should().ContainSingle(x =>
            x.PropertyName == "Content" &&
            x.ErrorCode == "MinimumLengthValidator" &&
            x.ErrorMessage == "The length of 'Content' must be at least 3 characters. You entered 2 characters." &&
            x.Severity == "Error");
    }

    private readonly Request_MockBuilder mockRequest = new();
    private Request request => mockRequest.Mock;
    private IEnumerable<ValidationFailure> issues;

    public ValidationPlugin_Design(ITestOutputHelper output) : base(output) { }
}

