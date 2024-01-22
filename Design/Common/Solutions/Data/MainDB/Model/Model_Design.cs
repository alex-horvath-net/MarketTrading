namespace Common.Solutions.Data.MainDB.Model;

public class Model_Design {
    [Fact]
    public void Tag_Design() {
        var id = 1;
        var name = "Name";
        var tag = new Tag(id, name);
        //tag = tag with { TagId = id, Name = name };
        tag.TagId.Should().Be(id);
        tag.Name.Should().Be(name);
    }

    [Fact]
    public void Post_Design() {
        var id = 1;
        var title = "Title";
        var content = "Content";
        var createdAt = DateTime.UtcNow;
        var post = new Post(id, title, content, createdAt);
        //post = post with { Id = id, Title = title, Content = content, CreatedAt = createdAt, Tags = new List<Tag>() };
        post.PostId.Should().Be(id);
        post.Title.Should().Be(title);
        post.Content.Should().Be(content);
        post.CreatedAt.Should().Be(createdAt);
        post.PostTags.Should().NotBeNull();
    }
}
