namespace Common.Solutions.Data.MainDB.DataModel;

public record Post(int PostId, string Title, string Content, DateTime CreatedAt)
{
    public List<PostTag> PostTags { get; set; } = [];
}
