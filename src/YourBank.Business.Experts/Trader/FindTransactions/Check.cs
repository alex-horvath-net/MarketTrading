using Business.Experts.Trader.EditTransaction;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

public class Check {
    public class Business(Business.IInfrastructure infra) : Feature.ICheck {
        public bool Run(Feature.Response response) {
            response.Enabled  = infra.IsEnabled();
            return !response.Enabled; 
        }
        public interface IInfrastructure { 
            bool IsEnabled();
        }

      
    }
    public class Infrastructure : Business.IInfrastructure {
        public bool IsEnabled() => false;
    }
}

public static class FlagExtensions {
    public static IServiceCollection AddCheck(this IServiceCollection services) => services
        .AddScoped<Feature.ICheck, Check.Business>()
        .AddScoped<Check.Business.IInfrastructure, Check.Infrastructure>();
}
