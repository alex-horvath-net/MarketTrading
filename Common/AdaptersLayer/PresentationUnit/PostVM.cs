namespace Common.AdaptersLayer.PresentationUnit;

public record PostVM
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<TagVM> Tags { get; set; }
}
