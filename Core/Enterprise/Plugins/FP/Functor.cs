using FluentAssertions;
using Xunit;

namespace Core.Enterprise.Plugins.FP;

public record Functor<T>(T Value)
{
    // Haskel: fmap
    // F#: missing
    // C# Select => query syntax 
    public Functor<R> Map<R>(Func<T, R> convert) => new(convert(Value));
    public Functor<R> FMap<R>(Func<T, R> convert) => Map(convert);
    public Functor<R> Select<R>(Func<T, R> convert) => Map(convert);

    public static implicit operator Functor<T>(T Value) => new Functor<T>(Value);
    public static implicit operator T(Functor<T> functor) => functor.Value;
}

public class Functor_Design
{
    [Fact]
    public void Maping_To_Itself_Makes_No_Change_With_Lambda()
    {
        var sut = new Functor<int>(42);
        var result = sut.Select(x => x);
        sut.Should().Be(result);
    }

    [Fact]
    public void Maping_To_Itself_Makes_No_Change_With_Linq()
    {
        var sut = new Functor<int>(42);
        var result =
            from x in sut
            select x;
        sut.Should().Be(result);
    }

    [Fact]
    public void Sequintial_And_Nested_Maping_Are_Identical_With_Lambda()
    {
        Func<int, string> map1 = i => i.ToString();
        Func<string, string> map2 = s => new string(s.Reverse().ToArray());
        var sut = new Functor<int>(42);
        var result1 = sut.Select(map1).Select(map2);
        var result2 = sut.Select(x => map2(map1(x)));
        sut.Should().NotBe(result1);
        result1.Should().Be(result2);
    }

    [Fact]
    public void Sequintial_And_Nested_Maping_Are_Identical_With_Linq()
    {
        Func<int, string> map1 = i => i.ToString();
        Func<string, string> map2 = s => new string(s.Reverse().ToArray());
        var sut = new Functor<int>(42);
        var result1 = sut.Select(map1).Select(map2);
        var result2 = sut.Select(x => map2(map1(x)));
        sut.Should().NotBe(result1);
        result1.Should().Be(result2);
    }
}
