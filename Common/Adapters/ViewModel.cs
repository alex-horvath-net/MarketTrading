namespace App.Adapters;

public record PostVM
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<TagVM> Tags { get; set; }
}

public record TagVM
{
    public int Id { get; set; }
    public string Name { get; set; }
}
