namespace Core.PluginLayer.FP
{
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
}
