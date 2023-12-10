namespace Core.PluginLayer.FP;

public record Monad<T>(T Value)
{
    //Generic Types ..=>Functors ..=>Applicative Finctors ..=>Monads
    //Monad is a functor you can flatten

    public Monad<R> Map<R>(Func<T, R> toR) => toR(Value);
    public Monad<R> FMap<R>(Func<T, R> toR) => Map(toR);
    public Monad<R> Select<R>(Func<T, R> toR) => Map(toR);

    public static implicit operator Monad<T>(T Value) => new(Value);
    public static implicit operator T(Monad<T> functor) => functor.Value;

    // C# FlatMap
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
    // Flatten
    // Haskel: join
    // C#: missing, so we create create a similar extension methods
    public static Monad<T> Flatten<T>(this Monad<Monad<T>> nestedSource) =>
        nestedSource.Value;
    public static Monad<T> Join<T>(this Monad<Monad<T>> source) =>
        source.Flatten();


}