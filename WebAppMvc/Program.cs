using Common;
using Common.Solutions.Data.MainDB;
using Core;
using Experts;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .AddMyLibraryService2();

builder.Services
    .AddCore()
    .AddCommon()
    .AddExperts()
    ;

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }