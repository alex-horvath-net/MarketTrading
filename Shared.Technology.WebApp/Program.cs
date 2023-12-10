using Blogger.ReadPosts;
using Blogger.ReadPosts.Adapters.ValidationUnit;
using Blogger.ReadPosts.Plugins.DataAccessUnit;
using Blogger.ReadPosts.Tasks.ValidationUnit;
using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Common.Plugins.DataAccessUnit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddCore(builder.Configuration);
builder.Services.AddReadPosts();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDataBase();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
