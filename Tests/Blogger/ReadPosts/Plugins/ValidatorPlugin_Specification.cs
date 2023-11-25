using Blogger.ReadPosts.Plugins;
using Tests.Blogger.ReadPosts.Business;

namespace Tests.Blogger.ReadPosts.Plugins;

public class ValidatorPlugin_Specification
{
    [Fact]
    public async void Valid_Request()
    {
        var unit = new ValidatorPlugin();
        var issues = await unit.Validate(feature.Request, feature.Token);

        issues.Should().NotBeNull();
        issues.Should().BeEmpty();
    }

    [Fact]
    public async void InValid_Request()
    {
        var unit = new ValidatorPlugin();
        var issues = await unit.Validate(feature.UseInvalidRequest().Request, feature.Token);

        issues.Should().NotBeNull();
        issues.Should().NotBeEmpty();
    }
    private readonly WorkFlow_MockBuilder feature = new();
}
