using Microsoft.Playwright;

namespace Clients.WebAppMvc;

public class Page : IClassFixture<WebAppFactory>, IAsyncLifetime {

    public Page(WebAppFactory appFactory, ITestOutputHelper output) {
        this.appFactory = appFactory;
        this.output = output;
    }
    public async Task InitializeAsync() {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        page = await browser.NewPageAsync();
    }

    public async Task DisposeAsync() {
        await page.CloseAsync();
        await browser.CloseAsync();
    }

    protected IPlaywright playwright;
    protected IBrowser browser;
    protected IPage page;
    protected readonly WebAppFactory appFactory;
    protected readonly ITestOutputHelper output;
}


/*
 * Package Mager Console
 * Install-Package Microsoft.Playwright
 * 
 * Developer Command Prompt
 * npx playwright install
 * 
 */