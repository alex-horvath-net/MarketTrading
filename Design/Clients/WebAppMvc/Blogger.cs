namespace Clients.WebAppMvc;
public class Blogger(WebAppFactoryMin appFactory, ITestOutputHelper output) : Page(appFactory, output) {
    [Fact]
    public async Task ExampleTest() {
        var app = appFactory.CreateClient();

        await page.GotoAsync( $"{app.BaseAddress}post");
        
        await page.TitleAsync().ContinueWith(x => x.Result.Should().Be("Posts - WebAppMvc"));
    }
}
