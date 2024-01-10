namespace Core.App.Sockets.DataModel;

public record Post(int PostId, string Title, string Content, DateTime CreatedAt) {
    public List<PostTag> PostTags { get; set; } = [];
}

public record Tag(int TagId, string Name) {
    public List<PostTag> PostTags { get; set; } = [];
}


public record PostTag(int PostId, int TagId) {
    public Post Post { get; init; }
    public Tag Tag { get; init; }
}