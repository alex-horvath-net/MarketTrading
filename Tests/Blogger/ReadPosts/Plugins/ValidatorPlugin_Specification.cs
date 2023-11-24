using Blogger.ReadPosts.Plugins;
using FluentAssertions;
using Tests.Blogger.ReadPosts.Business;

namespace Tests.Blogger.ReadPosts.Plugins;

public class ValidatorPlugin_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new ValidatorPlugin();
        var response = await unit.Validate(feature.Request, feature.Token);

        response.Should().NotBeNull();
    }

    private readonly WorkFlow_MockBuilder feature = new();
}
