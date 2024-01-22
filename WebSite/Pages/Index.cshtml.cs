using Microsoft.AspNetCore.Mvc.RazorPages;
using Experts.Blogger.ReadPosts;
using Experts.Blogger;

namespace WebSite.Pages;
public class IndexModel(Expert blogger, ILogger<IndexModel> logger) : PageModel
{

    public async Task OnGetAsync()
    {
        var posts = await blogger.ReadPosts.Run(new  Story.Request("Title", "Content"), CancellationToken.None);
    }
}