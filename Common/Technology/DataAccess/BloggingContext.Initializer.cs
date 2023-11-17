using Shared.Adapter.DataModel;

namespace Shared.Technology.DataAccess;

public partial class BloggingContext
{
    public void EnsureInitialized()
    {
        var seeded = Posts.Any();
        if (seeded) return;

        InitTags();
        InitPosts();
    }

    private void InitTags()
    {
        var tags = new Tag[]
        {
            new Tag{Id=1,Name="Tag1" },
            new Tag{Id=2,Name="Tag2" },
            new Tag{Id=3,Name="Tag3" },
        };
        Tags.AddRange(tags);
        SaveChanges();
    }

    private void InitPosts()
    {
        var posts = new Post[]
        {
            new Post{ Id=1, Title="Title1",Content="Content1",CreatedAt=DateTime.Parse("2023-12-01")},
            new Post{ Id=2, Title="Title2",Content="Content2",CreatedAt=DateTime.Parse("2023-12-02")},
            new Post{ Id=3, Title="Title3",Content="Content3",CreatedAt=DateTime.Parse("2023-12-03")}
        };
        Posts.AddRange(posts);
        SaveChanges();
    }
}
