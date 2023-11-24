using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blogger.ReadPosts.Business;
using Blogger.ReadPosts.PluginAdapters;
using FluentAssertions;
using NSubstitute;
using Tests.Blogger.ReadPosts.Business;

namespace Tests.Blogger.ReadPosts.PluginAdapters;

public class Validate_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new ValidatorPluginAdapter(validator.Mock);
        var response = await unit.Validate(feature.Request, feature.Token);

        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => validator.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
        await validator.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
    }

    private readonly ValidatorPlugin_MockBuilder validator = new();
    private readonly WorkFlow_MockBuilder feature = new();
}
