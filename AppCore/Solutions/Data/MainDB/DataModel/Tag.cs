namespace Common.Solutions.Data.MainDB.DataModel;

public record Tag(int TagId, string Name)
{
    public List<PostTag> PostTags { get; set; } = [];
}
