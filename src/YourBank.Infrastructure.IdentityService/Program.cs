using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Handles user authentication and authorization.
// It integrates with Azure AD for internal users and manages local accounts for external stakeholders.
// Issues your own “YourBank” JWT tokens.
    
    builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
