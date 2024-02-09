using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.Core;
using Microsoft.Playwright.Transport;
using Microsoft.Playwright.Transport.Protocol;
using Xunit;
using Xunit.Extensions;
namespace UI.WebAppMvc;

public class Page2  {


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
    }
    public async Task InitializeAsync() {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        page = await browser.NewPageAsync();
        await page.GotoAsync(Url);
    }

    public async Task DisposeAsync() {
        await page.CloseAsync();
        await browser.CloseAsync();
    }

    public string Url => $"{appFactory.ServerAddress}{pageUrl}";
    protected IPlaywright playwright;
    protected IBrowser browser;
    protected IPage page;
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