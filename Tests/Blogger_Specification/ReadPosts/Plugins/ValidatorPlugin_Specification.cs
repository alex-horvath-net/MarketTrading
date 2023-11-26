using Blogger.ReadPosts.Plugins;
using Specifications.Blogger_Specification.ReadPosts.Business;

namespace Specifications.Blogger_Specification.ReadPosts.Plugins;

public class ValidatorPlugin_Specification
{
    [Fact]
    public async void Valid_Request()
    {
        var unit = new Validation();
        var issues = await unit.Validate(feature.Request, feature.Token);

        issues.Should().NotBeNull();
        issues.Should().BeEmpty();
    }

    [Fact]
    public async void InValid_Request()
    {
        var unit = new Validation();
        var issues = await unit.Validate(feature.UseInvalidRequest().Request, feature.Token);

        issues.Should().NotBeNull();
        issues.Should().NotBeEmpty();
    }
    private readonly Featrue_MockBuilder feature = new();
}
