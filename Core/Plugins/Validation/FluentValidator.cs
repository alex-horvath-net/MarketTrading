using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Core.Plugins.Validation;

public abstract class FluentValidator<T> : FluentValidation.AbstractValidator<T>
{
    public async Task<IEnumerable<Core.Sockets.Validation.ValidationFailure>> Validate(T request, CancellationToken token)
    {
        var pluginModel = await ValidateAsync(request, token);
        var socketModel = pluginModel.Errors.Select(Map);
        return socketModel;
    }

    private Core.Sockets.Validation.ValidationFailure Map(FluentValidation.Results.ValidationFailure plugin) => new(
        plugin.PropertyName,
        plugin.ErrorCode,
        plugin.ErrorMessage,
        plugin.Severity.ToString());
}

public class FluentValidator_Design
{
    [Fact]
    public async void TestValidator_OneFailure()
    {
        var validatorPlugin = new TestValidator();
        var request = new TestRequest("1234");
        var token = CancellationToken.None;

        var socketModel = await validatorPlugin.Validate(request, token);

        socketModel.Should().ContainSingle(e =>
            e.PropertyName == "Name" &&
            e.ErrorCode == "MinimumLengthValidator" &&
            e.ErrorMessage == "The length of 'Name' must be at least 5 characters. You entered 4 characters." &&
            e.Severity == "Error");
    }

    [Fact]
    public async void TestValidator_NoFailure()
    {
        var validatorPlugin = new TestValidator();
        var request = new TestRequest("12345");
        var token = CancellationToken.None;

        var socketModel = await validatorPlugin.Validate(request, token);

        socketModel.Should().BeEmpty();
    }

    public class TestValidator : FluentValidator<TestRequest>
    {
        public TestValidator() => RuleFor(x => x.Name).MinimumLength(5);
    }
    public record TestRequest(string Name);
}

