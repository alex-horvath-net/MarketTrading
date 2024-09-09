using FluentValidation;

namespace Experts.Trader.FindTransactions.Validate.Technology;

public class Validator : AbstractValidator<Request> {
    public Validator() {
        RuleFor(x => x).NotNull().WithMessage(RequestIsNull);
        RuleFor(x => x.Name).MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(NameIsShort);
    }

    public static string RequestIsNull => "Request must be provided.";
    public static string NameIsShort => "Name must be at least 3 characters long if provided.";
}
