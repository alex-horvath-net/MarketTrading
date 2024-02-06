using Core.Solutions.Validation;
using Experts.Blogger.ReadPosts.Business.Model;
using FluentValidation;

namespace Experts.Blogger.ReadPosts.Solutions;

public class Validation : ValidationCore<Request>, Business.IValidator {
    public Validation() {
        RuleFor(request => request.Filter)
          .Cascade(CascadeMode.Stop)
          .Must(x => x == null || x.Length >= 3)
          .WithMessage("Title must be null or at least 3 characters long.");
    }
}

