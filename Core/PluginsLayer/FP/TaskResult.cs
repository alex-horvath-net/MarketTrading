namespace Principals.PluginsLayer.FP;

public record TaskBox<T>(T Content)
{
    public TaskBox<R> Map<R>(Func<T, R> mapT2R) => new TaskBox<R>(mapT2R(Content));

    public TaskBox<R> Bind<R>(Func<T, TaskBox<R>> mapT2TaskBoxR) => mapT2TaskBoxR(Content);

    public static TaskBox<T> Return(T content) => new TaskBox<T>(content);
}

public static class TaskTaskBoxExtensions
{
    public static TaskBox<R> Select<T, R>(this TaskBox<T> TaskBoxT,
        Func<T, R> mapT2R) => TaskBoxT.Map(mapT2R);

    public static TaskBox<R> SelectMany<T, R>(this TaskBox<T> TaskBoxT,
        Func<T, TaskBox<R>> mapT2TaskBoxR) => TaskBoxT.Bind(mapT2TaskBoxR);

    public static TaskBox<R> SelectMany<T, U, R>(this TaskBox<T> TaskBoxT,
        Func<T, TaskBox<U>> mapT2TaskBoxU,
        Func<T, U, R> mapTU2R) => TaskBoxT.Bind(t => mapT2TaskBoxU(t).Map(u => mapTU2R(t, u)));
}
