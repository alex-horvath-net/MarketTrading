using Xunit;

namespace Shared;

public static class TestExtensions
{
    public static T Inc<T>(this T actual, int counter)
    {
        return actual;
    }

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

    public static T? ShouldBe<T>(this T? actual, T? expected)
    {
        Assert.Equal(expected, actual);
        return actual;
    }
}
