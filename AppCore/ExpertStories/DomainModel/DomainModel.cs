namespace Common.ExpertStories.DomainModel;

public record Post {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Tag> Tags { get; set; }
}

public record Tag(int Id, string Name);