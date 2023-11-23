using Core.Business.ValidationModel;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace Blogger.ReadPosts.PluginAdapters;

public class ValidatorPluginAdapter(
    PluginAdapters.IValidatorPlugin validatorPlugin) : Business.IValidatorPluginAdapter
{
    public async Task<IEnumerable<ValidationResult>> Validate(Business.IFeature.Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }

    public class Validate_Specification
    {
        [Fact]
        public async void Path_Without_Diversion()
        {
            var unit = new PluginAdapters.ValidatorPluginAdapter(validator.Mock);
            var response = await unit.Validate(feature.Request, feature.Token);

            response.Should().NotBeNullOrEmpty();
            response.Should().OnlyContain(result => validator.Results.Any(x => x.ErrorCode == result.ErrorCode && x.ErrorMessage == result.ErrorMessage));
            await validator.Mock.ReceivedWithAnyArgs(1).Validate(default, default);
        }

        private readonly IValidatorPlugin.MockBuilder validator = new();
        private readonly Business.IFeature.MockBuilder feature = new();
    }
}
