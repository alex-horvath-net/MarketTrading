using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Common.Validation.FluentValidator.Adapters;
using Common.Validation.FluentValidator.Technology;
using Microsoft.Extensions.DependencyInjection;

namespace Common;
public static class Extensions {

    public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
        .AddScoped<ICommonClient<TRequest>, CommonClient<TRequest>>();

    public static string? GetName<T>(this Expression<Func<T, object>> expression) {
        var member =
            expression.Body is MemberExpression body ? body.Member :
            expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand ? operand.Member :
            throw new ArgumentException("Expression is not a member access", nameof(expression));

        return member?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? member?.Name;
    }

    public static object GetValue<T>(this Expression<Func<T, object>> column, T row) => column.Compile().Invoke(row);
}
