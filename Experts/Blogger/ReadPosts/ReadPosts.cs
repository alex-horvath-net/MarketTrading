using Common;
using Common.Model;
using Common.Solutions.Data.MainDB;
using Common.Solutions.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts;


public class Story(IValidation validation, IRepository repository) : Story<Request, Response>() {
    public override async Task RunCore(Response response, CancellationToken token) {
        await ExpertTask("Validate", response, async response => {
            response.ValidationResults = await validation.Validate(response.Request, token);
            response.Terminated = response.ValidationResults.Any(x => !x.IsSuccess);
        });
        
        if (response.Terminated)
            return;
        
        await ExpertTask("Read", response, async response => {
            response.Posts = await repository.Read(response.Request, token);
        }); 
    }

   
}

public record Request(string Title, string Content) : global::Common.Model.Request {
    public static Request Empty() => new(default, default);
}


public record Response() : Response<Request> {
    public static Response Empty() => new();
    public IEnumerable<Post>? Posts { get; set; }
}


public interface IValidation {
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);
}


public interface IRepository {
    Task<IEnumerable<Story.Model.Post>> Read(Request Request, CancellationToken token);
}


public class Validation : FluentValidator<Request>, IValidation {
    public Validation() {
        RuleFor(request => request.Title)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Title)}' can not be empty if '{nameof(request.Content)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator);

        RuleFor(request => request.Content)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Content)}' can not be empty if '{nameof(request.Title)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator);
    }
}


public class Repository(MainDB db) : IRepository {
    public async Task<IEnumerable<global::Common.Model.Post>> Read(Request request, CancellationToken token) {
        var solutionModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var problemModel = solutionModel
            .Select(model => new global::Story.Model.Post() {
                Title = model.Title,
                Content = model.Content
            });

        return problemModel;
    }
}