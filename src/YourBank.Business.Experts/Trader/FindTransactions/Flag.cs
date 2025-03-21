using Business.Experts.Trader.EditTransaction;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

public class Flag {
    public class Adapter(Adapter.IInfrastructure infra) : Featrure.IFlag {
        public bool IsPublic(Featrure.Request request, CancellationToken token) {
            var isPublic = infra.IsEnabled();
            token.ThrowIfCancellationRequested();
            return isPublic;
        }
        public interface IInfrastructure {
            bool IsEnabled();
        }

      
    }
    public class Infrastructure : Adapter.IInfrastructure {
        public bool IsEnabled() => false;
    }
}

public static class FlagExtensions {
    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<Featrure.IFlag, Flag.Adapter>()
        .AddScoped<Flag.Adapter.IInfrastructure, Flag.Infrastructure>();
}
