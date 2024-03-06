using Microsoft.AspNetCore.Mvc;
using WebAppMvc.Models;

namespace WebAppMvc.Controllers;

public class PostController(
    Experts.Blogger.Expert blogger,
    Experts.Blogger.ReadPosts.Presenter presenter,
    Core.Business.ILog<PostController> logger) : Controller {
    // GET: Posts
    public async Task<IActionResult> Index(string filterText = null) { //}, int? page, CancellationToken token) { 
        logger.Inform("{Action}", nameof(Index));
        var storyModel = await blogger.ReadPosts.Run(new(filterText), tokenSource.Token);
        return View(presenter.ViewModel);
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> DetailsAsync(int? id) {
        if (id == null) {
            return NotFound();
        }

        var resposne = await blogger.ReadPosts.Run(new($"{id}"), tokenSource.Token);
        var post = resposne;
        if (post == null) {
            return NotFound();
        }

        return View(post);
    }

    // GET: Posts/Create
    public IActionResult Create() {
        return View();
    }

    // POST: Posts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync([Bind("PostId,Title,Content,CreatedAt")] Post post) {
        if (ModelState.IsValid) {
            var resposne = await blogger.ReadPosts.Run(new($"{post}"), tokenSource.Token);
            var createdPost = resposne;
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> EditAsync(int? id) {
        if (id == null) {
            return NotFound();
        }

        var resposne = await blogger.ReadPosts.Run(new($"{id}"), tokenSource.Token);
        var post = resposne;
        if (post == null) {
            return NotFound();
        }
        return View(post);
    }

    // POST: Posts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(int id, [Bind("PostId,Title,Content,CreatedAt")] Post post) {
        if (id != post.PostId) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            var resposne = await blogger.ReadPosts.Run(new($"{post}"), tokenSource.Token);
            var updatedPost = resposne;

            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Delete/5
    public async Task<IActionResult> DeleteAsync(int? id) {
        if (id == null) {
            return NotFound();
        }

        var resposne = await blogger.ReadPosts.Run(new($"{id}"), tokenSource.Token);
        var post = resposne;
        if (post == null) {
            return NotFound();
        }

        return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmedAsync(int id) {
        var resposne = await blogger.ReadPosts.Run(new($"{id}"), tokenSource.Token);
        var post = resposne;
        return RedirectToAction(nameof(Index));
    }

    private CancellationTokenSource tokenSource = new();
}

