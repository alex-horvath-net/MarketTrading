namespace Clients.WebAppMvc;
public class Blogger(WebAppFactory appFactory, ITestOutputHelper output) : Page(appFactory, output) {
    [Fact]
    public async Task ExampleTest() {
        var app = appFactory.CreateClient();

        await page.GotoAsync( $"{app.BaseAddress}/post");
        
        await page.TitleAsync().ContinueWith(x => x.Result.Should().Be("Posts - WebAppMvc"));
    }
}


/*
 * Package Mager Console
 * Install-Package Microsoft.Playwright
 * 
 * Developer Command Prompt
 * npx playwright install
 * 
 */