namespace Common.Solutions.View.Model;

public record Post {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Tag> Tags { get; set; }
}

public record Tag {
    public int Id { get; set; }
    public string Name { get; set; }
}
