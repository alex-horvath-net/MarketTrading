
using Microsoft.EntityFrameworkCore;
using Shared.Technology.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var databaseName = "Blogging";
var connectionString = builder.Configuration.GetConnectionString(databaseName);
builder.Services.AddDbContext<BloggingContext>(options => options.UseInMemoryDatabase(databaseName));
//builder.Services.AddDbContext<BloggingContext>(options => options.UseSqlite(connectionString));
//builder.Services.AddDbContext<BloggingContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<BloggingContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.EnsureInitialized();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
