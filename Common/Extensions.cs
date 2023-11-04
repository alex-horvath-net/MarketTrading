namespace Common
{
    public static class Extensions
    {
        public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);
    }
}
