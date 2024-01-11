using BusinessExperts.Blogger;
using Common;
using Common.Solutions.DataAccess;
using Core;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddRazorPages();

builder
    .Services
    .AddCoreSystem()
    .AddCoreApplication(builder.Configuration, builder.Environment.IsDevelopment())
    .AddBlogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseDeveloperDataBase();
} else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
