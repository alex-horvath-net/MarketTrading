using Models.AdaptersLayer.PresentationUnit;

namespace Spec.Core_Specification.PluginAdapters;

public class ViewModel_Specification
{
    //[Fact]
    public void Tag()
    {
        var id = 1; var name = "Name";
        var tag = new TagVM();
        tag = tag with { Id = id, Name = name };
        tag.Id.Should().Be(id);
        tag.Name.Should().Be(name);
    }

    //[Fact]
    public void Post()
    {
        var id = 1; var title = "Title"; var content = "Content"; var createdAt = DateTime.UtcNow;
        var post = new PostVM();
        post = post with { Id = id, Title = title, Content = content, CreatedAt = createdAt, Tags = new List<TagVM>() };
        post.Id.Should().Be(id);
        post.Title.Should().Be(title);
        post.Content.Should().Be(content);
        post.CreatedAt.Should().Be(createdAt);
        post.Tags.Should().NotBeNull();
    }
}
