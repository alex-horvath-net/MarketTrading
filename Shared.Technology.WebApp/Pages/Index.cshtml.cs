using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessExperts.Blogger.ReadPosts;

namespace WebSite.Pages; 
public class IndexModel(
    BusinessExperts.Blogger.Expert blogger, 
    ILogger<IndexModel> logger) : PageModel {

    public async Task OnGetAsync() {
        var posts = await blogger.ReadPosts.Run(new Request("Title", "Content"), CancellationToken.None);  
    }
}