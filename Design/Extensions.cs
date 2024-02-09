namespace Design;
public static class Extensions {
    public static async Task ShouldBe<T>(this Task<T> task, string expected) {
        var actual = await task;
        actual.Should().Be(expected);
    }
}
