using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace YourBank.Infrastructure.Extensions;

public static class ReflectionExtensions {

    public static string? GetName<T>(this Expression<Func<T, object>> expression) {
        var member =
            expression.Body is MemberExpression body ? body.Member :
            expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand ? operand.Member :
            throw new ArgumentException("Expression is not a member access", nameof(expression));

        var name = member?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? member?.Name;
        return name;
    }

    public static object GetValue<T>(this Expression<Func<T, object>> column, T row) => column.Compile().Invoke(row);
}
