using Common.Business.Model;
using Common.Solutions.Data.MainDB;
using Core.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts.WorkSteps;

public class ReadPosts(ReadPosts.IRepository repository) : UserWorkStep<UserStoryRequest, UserStoryResponse> {
    public async Task<bool> Run(UserStoryResponse response, CancellationToken token) {
        response.Posts = await repository.Read(response.MetaData.Request, token);
        return true;
    }

    public interface IRepository {
        Task<IEnumerable<Post>> Read(UserStoryRequest request, CancellationToken token);
    }

    public class Repository(MainDB db) : IRepository {
        public async Task<IEnumerable<Post>> Read(UserStoryRequest request, CancellationToken token) {
            var solutionModel = await db.Posts
                .Include(post => post.PostTags)
                .ThenInclude(postTag => postTag.Tag)
                .Where(post =>
                    request.Filter == null ||
                    post.Title.Contains(request.Filter) ||
                    post.Content.Contains(request.Filter) ||
                    post.PostTags.Any(pt => pt.Tag.Name.Contains(request.Filter)))
                .ToListAsync(token);

            var businsessModel = solutionModel
                .Select(model => new Post() {
                    Id = model.PostId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedAt = model.CreatedAt,
                    Tags = model.PostTags.Select(pt => new Tag(pt.Tag.TagId, pt.Tag.Name)).ToList(),
                });

            return businsessModel;
        }
    }
}

public static class ReadPostsUserWorkStepExtensions {
    public static IServiceCollection AddReadPostsUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<UserStoryRequest, UserStoryResponse>, ReadPosts>()
        .AddScoped<ReadPosts.IRepository, ReadPosts.Repository>();
}
