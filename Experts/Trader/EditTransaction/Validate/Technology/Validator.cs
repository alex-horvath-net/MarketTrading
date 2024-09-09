using FluentValidation;

namespace Experts.Trader.EditTransaction.Validate.Technology;

public class Validator : AbstractValidator<Request> {
    public Validator() {
        RuleFor(x => x).NotNull().WithMessage(RequestIsNull);
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(IdMustBePositive);
        RuleFor(x => x.Name).MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(NameIsShort);
    }

    public static string RequestIsNull => "Request must be provided.";
    public static string IdMustBePositive => "Id mus be pozitive number.";
    public static string NameIsShort => "Name must be at least 3 characters long if provided.";
}
