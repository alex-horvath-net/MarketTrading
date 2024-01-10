namespace Core.App.Sockets.DataModel;

public record Post(int PostId, string Title, string Content, DateTime CreatedAt) {
    public List<Tag> Tags { get; set; } = [];
}

public record Tag(int TagId, string Name) {
    public List<Post> Posts { get; set; } = [];
}

