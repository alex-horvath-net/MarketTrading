using AppPolicy.Plugins.Validation;
using FluentValidation;

namespace Design.AppPolicy.Plugins.Validation;

public class FluentValidationDesign
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

