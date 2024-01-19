namespace Common.Solutions.Data.MainDB.Model;


public record PostTag(int PostId, int TagId) {
    public Post Post { get; init; }
    public Tag Tag { get; init; }
}