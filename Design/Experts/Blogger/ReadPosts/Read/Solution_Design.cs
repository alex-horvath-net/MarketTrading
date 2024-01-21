//using Core;
//using Microsoft.AspNetCore.Builder;
//using Story.Solutions.Data.MainDB;
//using Story;
//using Microsoft.Extensions.Hosting;


//namespace Experts.Blogger.ReadPosts.Read;

//public class Solution_Design(ITestOutputHelper output) : Design<Solution>(output) {
//    private void Create() => Unit = new Solution(db);

//    private async Task Act() => posts = await Unit.Read(request, token);

//    [Fact]
//    public void ItRequires_Dependecies() {
//        Create();

//        Unit.Should().NotBeNull();
//        Unit.Should().BeAssignableTo<ISolution>();
//    }

//    [Fact]
//    public async Task ItCan_Read() {
//        request = request.MockValidRequest();
//        Create();

//        await Act();

//        posts.Should().NotBeEmpty();
//    }


//    [Fact]
//    public void UseDataBase() {
//        var appBuilder = WebApplication.CreateBuilder();
//        appBuilder.Services.AddMainDB(appBuilder.Configuration, Environments.Development);
//        var app = appBuilder.Build();

//        var mainDB = app.UseDeveloperDataBase();

//        mainDB.Posts.Should().NotBeEmpty();
//    }

//    private IEnumerable<Story.Model.Post>? posts;
//    private readonly MainDB db = new MainDB().Schema(false);
//    private Request request = Request.Empty();
//}


