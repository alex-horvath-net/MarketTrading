namespace Infrastructure.Extensions;

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