using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
            _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} IndexModel is created.");
        }

        public async Task OnGetAsync() {
            _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} start OnGetAsync.");
            _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} end OnGetAsync.");
        }
    }
}