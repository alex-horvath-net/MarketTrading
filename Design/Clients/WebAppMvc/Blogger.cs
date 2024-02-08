namespace Clients.WebAppMvc;
public class Blogger(WebAppFactoryMin appFactory, ITestOutputHelper output) : Page(appFactory, output) {
    [Fact]
    public async Task ExampleTest() {
        var app = appFactory.CreateClient();
        var url = $"{app.BaseAddress}/post";
        await page.GotoAsync(url);

        await page.TitleAsync().ContinueWith(x => x.Result.Should().Be("Posts - WebAppMvc"));
    }
}
