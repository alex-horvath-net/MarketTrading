using Common;
using Common.SolutionExperts.DataModel;
using Common.Solutions.DataAccess;
using Core;
using Microsoft.AspNetCore.Builder;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Solution_Design(ITestOutputHelper output) : Design<Solution>(output)
{
    private void Create() => Unit = new Solution(db);

    private async Task Use() => posts = await Unit.Read(title, content, Token);

    [Fact]
    public void ItRequires_Dependecies()
    {
        db = databasePovider.GetTestDB(true);
        Create();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<ISolution>();
    }

    [Fact]
    public async Task ItCan_Read()
    {
        db = databasePovider.GetTestDB();

        Create();

        await Use();

        posts.Should().NotBeEmpty();
    }


    [Fact]
    public void UseDataBase()
    {
        var appBuilder = WebApplication.CreateBuilder();
        appBuilder.Services.AddCoreApplication(appBuilder.Configuration, isDevelopment: true);
        var app = appBuilder.Build();

        var db = app.UseDeveloperDataBase();

        db.Posts.Should().NotBeEmpty();
    }

    private List<Post>? posts;
    private DB? db;
    private DBProvider databasePovider = new();
    string title = "Title";
    string content = "Content";

}
