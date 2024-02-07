using Microsoft.Playwright;

namespace Clients.WebAppMvc;
public class Blogger {
    [Fact]
    public async Task ExampleTest() {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync();

        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://localhost:7287/post");

        var title = await page.TitleAsync();
        title.Should().Be("Example Domain");

        // Cleanup resources
        await browser.CloseAsync();
    }
}
