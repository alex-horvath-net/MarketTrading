namespace Common.Solutions.Data.MainDB.Model;

public record Tag(int TagId, string Name) {
    public List<PostTag> PostTags { get; set; } = [];
}
