using Core.Application.Sockets.ViewModel;
using FluentAssertions;
using Xunit;

namespace Core.Application.Sockets.ViewModel.Design;

public class ViewModel_Specification
{
    [Fact]
    public void Tag()
    {
        var id = 1;
        var name = "Name";
        var tag = new Tag();
        tag = tag with { Id = id, Name = name };
        tag.Id.Should().Be(id);
        tag.Name.Should().Be(name);
    }

    [Fact]
    public void Post()
    {
        var id = 1;
        var title = "Title";
        var content = "Content";
        var createdAt = DateTime.UtcNow;
        var post = new Post();
        post = post with { Id = id, Title = title, Content = content, CreatedAt = createdAt, Tags = new List<Tag>() };
        post.Id.Should().Be(id);
        post.Title.Should().Be(title);
        post.Content.Should().Be(content);
        post.CreatedAt.Should().Be(createdAt);
        post.Tags.Should().NotBeNull();
    }
}
