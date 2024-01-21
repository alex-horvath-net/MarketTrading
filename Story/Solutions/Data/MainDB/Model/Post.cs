namespace Story.Solutions.Data.MainDB.Model;

public record Post(int PostId, string Title, string Content, DateTime CreatedAt) {
    public List<PostTag> PostTags { get; set; } = [];
}
