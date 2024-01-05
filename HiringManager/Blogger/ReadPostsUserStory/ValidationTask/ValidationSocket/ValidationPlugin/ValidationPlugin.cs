
using FluentValidation;
using Newtonsoft.Json.Linq;

namespace Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationPlugin;

public class ValidationPlugin : FluentValidator<Request>, ValidationSocket.IValidationPlugin
{
    public ValidationPlugin()
    {
        RuleFor(request => request.Title)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Title)}' can not be empty if '{nameof(request.Content)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator);

        RuleFor(request => request.Content)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Content)}' can not be empty if '{nameof(request.Title)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator);
    }

    public class Design : Design<ValidationPlugin>
    {
        private void Construct() => Unit = new ValidationPlugin();

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

        private readonly Request.MockBuilder mockRequest = new();
        private Request request =>mockRequest.Mock;
        private IEnumerable<ValidationFailure> issues;

        public Design(ITestOutputHelper output) : base(output)
        {
        }
    }
}
