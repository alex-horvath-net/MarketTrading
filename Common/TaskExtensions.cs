namespace Shared
{
    public static class TaskExtensions
    {
        public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);
    }
}
