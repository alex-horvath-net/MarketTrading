namespace Core.Enterprise.Plugins.FP;

public static class BoxExtensions
{
    public static Box<T> ToBox<T>(this T content) => new Box<T>(content);

    public static Box<T> Join<T>(this Box<Box<T>> box) =>
        box.IsEmpty || box.Content.IsEmpty ?
        new Box<T>() :
        box.Content;

    public static Box<R> Select<T, R>(this Box<T> box, Func<T, R> mapT2R)
    {
        if (box.IsEmpty)
            return new Box<R>();

        var t = box.Content;
        var r = mapT2R(t);
        return r;
    }

    public static Box<R> SelectMany<T, R>(this Box<T> box, Func<T, Box<R>> mapT2BoxR)
    {
        if (box.IsEmpty)
            return new Box<R>();

        var t = box.Content;
        var boxR = mapT2BoxR(t);
        return boxR;
    }

    public static Box<R> SelectMany<T, U, R>(this Box<T> box, Func<T, Box<U>> mapT2BoxU, Func<T, U, R> mapTU2R)
    {
        if (box.IsEmpty)
            return new Box<R>();

        var t = box.Content;
        var boxU = mapT2BoxU(t);
        var u = boxU.Content;
        var r = mapTU2R(t, u);
        return r;
    }
}
