using Microsoft.AspNetCore.Mvc.RazorPages;
using Experts.Blogger.ReadPosts.Model;
using Experts.Blogger;

namespace WebSite.Pages;
public class IndexModel(Expert blogger, ILogger<IndexModel> logger) : PageModel
{

    public async Task OnGetAsync()
    {
        var posts = await blogger.ReadPosts.Run(new Request("Title", "Content"), CancellationToken.None);
    }
}