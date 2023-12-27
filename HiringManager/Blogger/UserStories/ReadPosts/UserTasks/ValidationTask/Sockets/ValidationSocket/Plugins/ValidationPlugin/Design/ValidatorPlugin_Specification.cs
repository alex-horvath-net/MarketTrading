using FluentAssertions;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin.Design;

public class ValidatorPlugin_Specification
{
    //[Fact]
    public async void Valid_Request()
    {
        var unit = new ValidationPlugin();
        var issues = await unit.Validate(request.UseValidRequest().Mock, CancellationToken.None);

        issues.Should().NotBeNull();
        issues.Should().BeEmpty();
    }

    //[Fact]
    public async void InValid_Request()
    {
        var unit = new ValidationPlugin();
        var issues = await unit.Validate(request.UseInvalidRequest().Mock, CancellationToken.None);

        issues.Should().NotBeNull();
        issues.Should().NotBeEmpty();
        issues.Should().OnlyContain(x => x.PropertyName == "");
        issues.Should().OnlyContain(x => x.ErrorCode == "PredicateValidator");
        issues.Should().OnlyContain(x => x.ErrorMessage == "Either Title or Content must be provided.");
        issues.Should().OnlyContain(x => x.Severity == "Error");
    }
    private readonly Request.MockBuilder request = new();
}
