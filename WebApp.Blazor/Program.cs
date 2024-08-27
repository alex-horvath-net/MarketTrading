using WebApp.Blazor.Components;
using Common.Technology.Identity;
using WebApp.Components;

namespace WebApp.Blazor;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddIdentityServices(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.UseMigrationsEndPoint();
        } else {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapAdditionalIdentityEndpoints(IdentityConstatansts.LoginCallbackAction, IdentityConstatansts.LinkLoginCallbackAction);

        app.Run();
    }
}
