using FluentAssertions;
using Xunit;

namespace Core.Plugins.FP;

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

public class Box_Design
{
    private string MapToItself(string itself) => itself;
    private int Parse(string text) => int.Parse(text);
    private DateTime ToDate(int year) => new DateTime(year, 1, 1);

    [Fact]
    public void CreateEmptyBox()
    {
        var box =  Box<string>.Empty;

        box.IsEmpty.Should().BeTrue();  
        box.Content.Should().Be(default);
    }

    [Fact]
    public void CreateNonEmptyBox()
    {
        var box = new Box<string>("1984");

        box.IsEmpty.Should().BeFalse();
        box.Content.Should().Be("1984");
    }

    [Fact]
    public void CreateImplicitNonEmptyBox()
    {
        Box<string> box = "1984";

        box.IsEmpty.Should().BeFalse();
        box.Content.Should().Be("1984");
    }

    [Fact]
    public void GetImplicitContentOfEmptyBox()
    {
        string content = "1984".ToBox();

        content.Should().Be("1984");
    }

    [Fact]
    public void ToBox()
    {
        var box = "1984".ToBox();

        box.Content.Should().Be("1984");
    }

    [Fact]
    public void Select_Lambda()
    {
        var box1 = "1984".ToBox();

        var box2 = box1.Select(Parse);

        box2.Content.Should().Be(1984);
    }
    [Fact]
    public void Select_Empty_Lambda()
    {
        var box1 = Box<string>.Empty;

        var box2 = box1.Select(Parse);

        box2.IsEmpty.Should().BeTrue();
    }
    [Fact]
    public void Select_Linq()
    {
        var box1 = "1984".ToBox();

        var box2 =
            from result1 in box1
            select Parse(result1);

        box2.Content.Should().Be(1984);
    }
    [Fact]
    public void Map_Functor_1()
    {
        var box = "1984".ToBox();
        var newBox = box.Select(MapToItself);

        newBox.Should().Be(box);
    }
    [Fact]
    public void Map_Functor_2()
    {
        var box = "1984".ToBox();
        var sequentialBox = box.Select(Parse).Select(ToDate);
        var nestedBox = box.Select(c => ToDate(Parse(c)));

        sequentialBox.Should().Be(nestedBox);
    }


    [Fact]
    public void Join()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        boxInBox.Content.Should().Be(box);
        boxInBox.Join().Should().Be(box);
    }

    [Fact]
    public void SelectMany_Lambda()
    {
        var box = "1984".ToBox().SelectMany(result1 => Parse(result1).ToBox());

        box.Content.Should().Be(1984);
    }

    [Fact]
    public void SelectMany_Empty_Lambda()
    {
        var box = Box<string>.Empty.SelectMany(result1 => Parse(result1).ToBox());

        box.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void SelectMany_Linq()
    {
        var box =
            from result1 in "1984".ToBox()
            from result2 in Parse(result1).ToBox()
            select result2;

        box.Content.Should().Be(1984);
    }
}
