using Core.Solutions.Validation;
using FluentValidation;

namespace Experts.Blogger.ReadPosts;

public interface IValidator : Core.Business.IValidator<Request> {
}

public class Validator : ValidationCore<Request>, IValidator {
    public Validator() {
        RuleFor(request => request.Filter)
          .Cascade(CascadeMode.Stop)
          .Must(x => x == null || x.Length >= 3)
          .WithMessage("Title must be null or at least 3 characters long.");
    }
}

