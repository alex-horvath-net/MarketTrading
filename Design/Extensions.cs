namespace Design;

public static class Extensions
{
    public static T? ShouldBe_NotNull<T>(this T? actual)
    {
        Assert.NotNull(actual);
        return actual;
    }

    public static IEnumerable<T> ShouldBe_NotEmpty<T>(this IEnumerable<T> actual)
    {
        Assert.NotEmpty(actual);
        return actual;
    }

    public static T? ShouldBe<T>(this T? actual, T expected)
    {
        Assert.Equal(expected, actual);
        return actual;
    }
}
