using Core;
using Core.Story.Model;
using Experts.Blogger.ReadPosts.Model;

namespace Experts.Blogger.ReadPosts.Validation;

public class Solution_Design(ITestOutputHelper output) : Design<Solution>(output) {
    private void Create() => Unit = new Solution();

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

    private Model.Request request = Model.Request.Empty();
    private IEnumerable<ValidationResult> issues;
}

