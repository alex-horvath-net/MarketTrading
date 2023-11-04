namespace Design;

public static class Extensions
{
    public static T? ShouldBe_NotNull<T>(this T? actual)
    {
        Assert.NotNull(actual);
        return actual;
    }
}
