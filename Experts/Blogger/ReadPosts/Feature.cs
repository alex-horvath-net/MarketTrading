using Common.Business.Model;
using Common.Solutions.Data.MainDB;
using Core.Business;
using Core.Solutions.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

#region Business

public record Request(string Title, string Content) : RequestCore() { }
public record Response() : ResponseCore<Request>() {
  public IEnumerable<Post>? Posts { get; set; }
}

public interface IUserStory : IUserStoryCore<Request, Response> { }

public interface IRepository {
  Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
}

public interface IValidator : Core.Business.IValidator<Request> { }

public class UserStory(
  IRepository repository,
  IValidator validator,
  ILogger<UserStory> logger) : StoryCore<Request, Response>(validator, logger, nameof(UserStory)), IUserStory {

  public override async Task Run(Response response, CancellationToken token) {
    response.Posts = await repository.Read(response.MetaData.Request, token);
  }
}

#endregion

#region Technology


public class Repository(MainDB db) : IRepository {
  public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
    var solutionModel = await db
      .Posts
      .Include(x => x.PostTags)
      .ThenInclude(x => x.Tag)
      .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
      .ToListAsync(token);

    var businsessModel = solutionModel
      .Select(model => new Common.Business.Model.Post() {
        Title = model.Title,
        Content = model.Content
      });

    return businsessModel;
  }
}

public class Validation : ValidationCore<Request>, IValidator {
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

public static class Extensions {
  public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
    .AddScoped<IUserStory, UserStory>()
    .AddScoped<IValidator, Validation>()
    .AddScoped<IRepository, Repository>();
}

#endregion