using Design;
using Microsoft.Playwright;

namespace UI.WebAppMvc;
public class Blogger(WebAppFactory appFactory, ITestOutputHelper output) : Page(appFactory, output, "post") {
    [Fact]
    public Task TitleIsSet() => page!
        .TitleAsync()
        .ShouldBe("Posts - WebAppMvc");

    [Fact]
    public Task TablePlaced() => page!
        .GetByRole(AriaRole.Table)
        .GetAttributeAsync("class")
        .ShouldBe("table");
}
