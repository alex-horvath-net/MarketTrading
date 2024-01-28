using Microsoft.AspNetCore.Mvc.RazorPages;
using Experts.Blogger.ReadPosts;
using Experts.Blogger;
using Microsoft.AspNetCore.Mvc;
using Common.Solutions.Data.MainDB.Configuration;
using Microsoft.Extensions.Options;

namespace WebSite.Pages;
//public class IndexModel(Expert blogger, ILogger<IndexModel> logger) : PageModel {
public class IndexModel : PageModel {
    //private readonly TransientFaultHandlingOptions options;

    //public IndexModel(IOptions<TransientFaultHandlingOptions> options) => this.options = options.Value;
    public async Task OnGetAsync() {
        //var posts = await blogger.ReadPosts.Run(new Request("Title", "Content"), CancellationToken.None);
        //logger.LogInformation("Posts {Posts}", posts);
    }
}