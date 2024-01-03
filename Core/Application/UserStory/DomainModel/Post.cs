using NSubstitute;

namespace Core.Application.UserStory.DomainModel;

public record Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Tag> Tags { get; set; }

    public class MockBuilder
    {
        public Post Mock { get; } = Substitute.For<Post>();

        public MockBuilder() => New();

        public MockBuilder New()
        {
             
            return this;
        }

        
    }
}

