namespace Core.PluginsLayer.FP;

public record Box<T>(T Content)
{
    public bool IsEmpty { get; }

    public static readonly Box<T> Empty = new();

    private Box() : this(default(T)) => IsEmpty = true;

    public static implicit operator Box<T>(T content) => content.ToBox();
    public static implicit operator T(Box<T> box) => box.Content;
}

public static class BoxExtensions
{
    public static Box<T> ToBox<T>(this T content) => new Box<T>(content);

    public static Box<T> Join<T>(this Box<Box<T>> newstedBoxT) => newstedBoxT.Content;

    public static Box<R> Select<T, R>(this Box<T> boxT, Func<T, R> mapT2R)
    {
        if (boxT.IsEmpty) return Box<R>.Empty;

        var t = boxT.Content;
        var r = mapT2R(t);
        return r;
    }

    public static Box<R> SelectMany<T, R>(this Box<T> boxT, Func<T, Box<R>> mapT2BoxR)
    {
        if (boxT.IsEmpty) return Box<R>.Empty;

        var t = boxT.Content;
        var boxR = mapT2BoxR(t);
        return boxR;
    }

    public static Box<R> SelectMany<T, U, R>(this Box<T> boxT, Func<T, Box<U>> mapT2BoxU, Func<T, U, R> mapTU2R)
    {
        if (boxT.IsEmpty) return Box<R>.Empty;

        var t = boxT.Content;
        var boxU = mapT2BoxU(t);
        var u = boxU.Content;
        var r = mapTU2R(t, u);
        return r;
    }
}
