using AlgoritmicTrading.Components;
using Common.Technology;
using Common.Technology.Identity;
using DomainExperts.Trader.EditTransaction;
using DomainExperts.Trader.FindTransactions;

namespace TradingWebSite;

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
