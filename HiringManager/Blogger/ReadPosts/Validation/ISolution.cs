namespace Experts.Blogger.ReadPosts.Validation;

public interface ISolution {
    Task<IEnumerable<Core.ExpertStory.StoryModel.Validation>> Validate(Request request, CancellationToken token);
}
