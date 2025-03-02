using YourBank.Infrastructure.TradingPortal.Components;
using YourBank.Business.Experts.Trader.FindTransactions.Feature;
using YourBank.Business.Experts.Trader.EditTransaction;
using YourBank.Infrastructure.Technology;
using YourBank.Infrastructure.Technology.Identity;

namespace YourBank.Infrastructure.TradingPortal;
// A front-end application (Blazor Server) for traders.
// It provides user interfaces for login, live market data, order entry, and status tracking.
public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCommonTechnology(builder.Configuration);
        builder.Services.AddFindTransactions(builder.Configuration);
        builder.Services.AddEditTransaction(builder.Configuration);

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

public interface IMarker {
}
