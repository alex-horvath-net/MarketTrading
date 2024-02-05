using Microsoft.AspNetCore.Mvc;

namespace WebAppMvc.Controllers {
  public class HomeController : Controller {
    public async Task<IActionResult> Index() => View();

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new WebAppMvc.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
