using Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
namespace UI.WebAppMvc;

public class Page2 {


    public Page2(ITestOutputHelper output) {
        this.output = output;
    }


    public async Task SetUp() {
        webApp = webAppFactory.CreateClient(new() { BaseAddress = new("http://127.0.0.1") });
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        page = await browser.NewPageAsync();
    }

    public async Task DisposeAsync() {
        await page.CloseAsync();
        await browser.CloseAsync();
    }

    protected IPlaywright playwright;
    protected IBrowser browser;
    protected IPage page;
    protected readonly WebApplicationFactory<Program> webAppFactory = new();
    protected HttpClient webApp;
    protected readonly ITestOutputHelper output;

}


public class Page : IClassFixture<WebAppFactory>, IAsyncLifetime {

    public Page(WebAppFactory appFactory, ITestOutputHelper output, string pageUrl) {
        this.appFactory = appFactory;
        this.output = output;
        this.pageUrl = pageUrl;
        output.WriteLine($"{DateTime.UtcNow:HH:mm:ss fff} Page Created.");
    }
    public async Task InitializeAsync() {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        page = await browser.NewPageAsync();
        using var _ = appFactory.CreateDefaultClient();
        var url = $"{appFactory.ClientOptions.BaseAddress}{pageUrl}";
        await page.GotoAsync(url);
        output.WriteLine($"{DateTime.UtcNow:HH:mm:ss fff} Page Initialized. {url}");
    }

    public async Task DisposeAsync() {
        await page.CloseAsync();
        await browser.CloseAsync();
        output.WriteLine($"{DateTime.UtcNow:HH:mm:ss fff} Page Dispozed");
    }
    
    protected IPlaywright? playwright;
    protected IBrowser? browser;
    protected IPage? page;
    protected readonly WebAppFactory appFactory;
    protected readonly ITestOutputHelper output;
    private readonly string pageUrl;
}


/*
 * Package Mager Console
 * Install-Package Microsoft.Playwright
 * 
 * Developer Command Prompt
 * npx playwright install
 * 
 */