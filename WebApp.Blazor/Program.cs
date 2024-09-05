using Common.Technology.Identity;
using Experts.Trader.FindTransactions;
using WebApp.Blazor.Components;

namespace AlgoritmicTrading;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddReadTransactions(builder.Configuration);

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
