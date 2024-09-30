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

}


public static class ReflectionExtensions {

    public static string? GetName<T>(this Expression<Func<T, object>> expression) {
        var member =
            expression.Body is MemberExpression body ? body.Member :
            expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand ? operand.Member :
            throw new ArgumentException("Expression is not a member access", nameof(expression));

        var name=  member?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? member?.Name;
        return name;
    }

    public static object GetValue<T>(this Expression<Func<T, object>> column, T row) => column.Compile().Invoke(row);


}

public static class TaskExtensions {
    public static Task<TInput> ToTask<TInput>(this TInput input) => Task.FromResult(input);


    public static async Task<TOutput> Select<TInput, TOutput>(this Task<TInput> inputTask,
        Func<TInput, TOutput> toOutput) {
        var input = await inputTask;
        var output = toOutput(input);
        return output;
    }

    public static async Task<TOutput> SelectMany<TInput, TOutput>(this Task<TInput> inputTask,
        Func<TInput, Task<TOutput>> toOutputTask) {
        var input = await inputTask;
        var output = await toOutputTask(input);
        return output;
    }

    public static async Task<TOutput> SelectMany<TInput, TMidle, TOutput>(this Task<TInput> inputTask,
        Func<TInput, Task<TMidle>> toMidleTask,
        Func<TInput, TMidle, TOutput> toOutput) {
        var input = await inputTask;
        var midle = await toMidleTask(input);
        var output = toOutput(input, midle);
        return output;
    }
}