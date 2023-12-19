using FluentAssertions;
using Xunit;

namespace Core.Plugins.FP;

public record Monad<T>(T Value)
{
    //Generic Types..=> Functors..=>Applicative Finctors..=> Monads
    //Monad is a functor you can flatten

    public Monad<R> Map<R>(Func<T, R> toR) => toR(Value);
    public Monad<R> FMap<R>(Func<T, R> toR) => Map(toR);
    public Monad<R> Select<R>(Func<T, R> toR) => Map(toR);

    public static implicit operator Monad<T>(T Value) => new(Value);
    public static implicit operator T(Monad<T> functor) => functor.Value;

    //C# FlatMap
    // F# Bind
    // Scala flatMap
    // Haskell >>=
    public Monad<R> Bind<R>(Func<T, Monad<R>> toMonadR) => // F#, nuggets
        Select(toMonadR).Flatten();
    public Monad<R> FlatMap<R>(Func<T, Monad<R>> toMonadR) =>  // scala
        Bind(toMonadR);
    public Monad<R> SelectMany<R>(Func<T, Monad<R>> toMonadR) =>
        Bind(toMonadR);
    public Monad<R> SelectMany<U, R>(Func<T, Monad<U>> toMonadU, Func<T, U, R> toR) =>
        Bind(t => toMonadU(t).Select(u => toR(t, u)));

    public static Monad<T> Return(T value) => value;
}

public static class Extensions
{
    //Flatten
    //Haskel: join
    //C#: missing, so we create create a similar extension methods
    public static Monad<T> Flatten<T>(this Monad<Monad<T>> nestedSource) =>
        nestedSource.Value;
    public static Monad<T> Join<T>(this Monad<Monad<T>> source) =>
        source.Flatten();


}

public class Specify_Monad
{
    [Fact]
    public void Maping_To_Itself_Makes_No_Change_With_Lambda()
    {
        var sut = new Monad<int>(42);
        var result = sut.Select(x => x);
        sut.Should().Be(result);
    }

    [Fact]
    public void Maping_To_Itself_Makes_No_Change_With_Linq()
    {
        var sut = new Monad<int>(42);
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
        var sut = new Monad<int>(42);
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
        var sut = new Monad<int>(42);
        var result1 = sut.Select(map1).Select(map2);
        var result2 = sut.Select(x => map2(map1(x)));
        sut.Should().NotBe(result1);
        result1.Should().Be(result2);
    }

    [Fact]
    public void Flatten()
    {
        var sut = new Monad<Monad<int>>(42);
        var result1 = sut.Flatten();
        var result2 = sut.Join();
        sut.Value.Value.Should().Be(result1.Value);
        result1.Should().Be(result2);
    }

    [Fact]
    public void Bind()
    {
        var sut = new Monad<int>(42);
        var result1 = sut.Bind(x => new Monad<string>(x.ToString()));
        var result2 = sut.SelectMany(x => new Monad<string>(x.ToString()));
        var result3 = sut.FlatMap(x => new Monad<string>(x.ToString()));

        sut.Value.ToString().Should().Be(result1.Value);
        result1.Should().Be(result2);
        result2.Should().Be(result3);
    }

    [Fact]
    public void Bind_Linq()
    {
        var sut1 = new Monad<int>(44);
        var sut2 = new Monad<int>(2);

        var result =
            from s1 in sut1
            from s2 in sut2
            select s1 - s2;

        result.Value.Should().Be(42);
    }
}
