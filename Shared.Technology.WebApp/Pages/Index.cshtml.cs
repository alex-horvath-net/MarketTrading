using Common.Plugins.TaskTry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Game game;

        public IndexModel(ILogger<IndexModel> logger, Game game)
        {
            _logger = logger;
            this.game = game;
            _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} IndexModel is created.");
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} start OnGetAsync.");
            await game.Play(200, CancellationToken.None).ConfigureAwait(false);
            _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} end OnGetAsync.");
        }
    }
}