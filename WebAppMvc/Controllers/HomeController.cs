using Microsoft.AspNetCore.Mvc;

namespace WebAppMvc.Controllers {
    public class HomeController(Experts.Blogger.Expert expert, Core.Business.ILogger<HomeController> logger) : Controller {
        public async Task<IActionResult> Index() {
            logger.LogInformation("{Action}", nameof(Index));
            var resposne = await expert.ReadPosts.Run(new("Title", "Content"), CancellationToken.None);
            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new WebAppMvc.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
