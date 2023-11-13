using Shared.Adapter.DataModel;

namespace Shared.Technology.Data;

public static class DbInitializer
{
    public static void Initialize(BloggingContext db)
    {
        var seeded = db.Posts.Any();
        if (seeded) return;

        var posts = new Post[]
        {
            new Post{ Id=1, Title="Title1",Content="Content1",CreatedAt=DateTime.Parse("2019-09-01")},
            new Post{ Id=2, Title="Title2",Content="Content2",CreatedAt=DateTime.Parse("2019-09-01")},
            new Post{ Id=3, Title="Title3",Content="Content3",CreatedAt=DateTime.Parse("2019-09-01")},
        };
        db.Posts.AddRange(posts);
        db.SaveChanges();

        var tags = new Tag[]
        {
            new Tag{Id=1,Name="Tag1" },
            new Tag{Id=2,Name="Tag2" },
            new Tag{Id=3,Name="Tag3" },
        };
        db.Tags.AddRange(tags);
        db.SaveChanges();

    }
}
