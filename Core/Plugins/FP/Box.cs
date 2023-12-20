using FluentAssertions;
using Xunit;

namespace Core.Plugins.FP;

public record Box<T>(T Content)
{
    public bool IsEmpty { get; }

    public static readonly Box<T> Empty = new();

    public Box<R> Join<R>() => !IsEmpty && Content is Box<R> boxR ? boxR : Box<R>.Empty;

    public Box<R> Select<R>(Func<T, R> mapT2R)
    {
        if (IsEmpty)
            return Box<R>.Empty;

        var t = Content;
        var r = mapT2R(t);
        return r;
    }

    public Box<R> SelectMany<R>(Func<T, Box<R>> mapT2BoxR)
    {
        if (IsEmpty)
            return Box<R>.Empty;

        var t = Content;
        var boxR = mapT2BoxR(t);
        return boxR;
    }

    public Box<R> SelectMany<U, R>(Func<T, Box<U>> mapT2BoxU, Func<T, U, R> mapTU2R)
    {
        if (IsEmpty)
            return Box<R>.Empty;

        var t = Content;
        var boxU = mapT2BoxU(t);
        var u = boxU.Content;
        var r = mapTU2R(t, u);
        return r;
    }

    private Box() : this(default(T)) => IsEmpty = true;

    public static implicit operator Box<T>(T content) => new(content);
    public static implicit operator T(Box<T> box) => box.Content;
}

public static class BoxExtensions
{
    public static Box<T> ToBox<T>(this T content) => new(content);

}

public class Box_Design
{
    private int String2Int(string text) => int.Parse(text);
    private DateTime Int2Date(int year) => new DateTime(year, 1, 1);

    [Fact]
    public void CreateEmptyBox()
    {
        var box = Box<string>.Empty;

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
        var box = "1984".ToBox().Select(String2Int);

        box.Content.Should().Be(1984);
    }
    [Fact]
    public void Select_Empty_Lambda()
    {
        var box = Box<string>.Empty.Select(String2Int);

        box.IsEmpty.Should().BeTrue();
    }
    [Fact]
    public void Select_Linq()
    {
        var box =
            from result1 in "1984".ToBox()
            select String2Int(result1);

        box.Content.Should().Be(1984);
    }
    [Fact]
    public void Map_Functor_1()
    {
        var box = "1984".ToBox();
        var reBox = box.Select(content => content);

        reBox.Should().Be(box);
    }
    [Fact]
    public void Map_Functor_2()
    {
        var box = "1984".ToBox();
        var reBoxIn2Step = box.Select(String2Int).Select(Int2Date);
        var reBoxIn1Step = box.Select(content => Int2Date(String2Int(content)));


        reBoxIn2Step.Should().Be(reBoxIn1Step);
    }


    
    [Fact]
    public void Join_WorngBoxe()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        boxInBox.Content.Should().Be(box);
        boxInBox.Join<int>().Should().Be(Box<int>.Empty);
    }

    [Fact]
    public void Join_RightButEmptyBox()
    {
        var box = Box<string>.Empty;
        var boxInBox = box.ToBox();

        boxInBox.Content.Should().Be(box);
        boxInBox.Join<string>().Should().Be(Box<string>.Empty);
    }


    [Fact]
    public void Join_RightBox()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        boxInBox.Content.Should().Be(box);
        boxInBox.Join<string>().Should().Be(box);
    }
    

    [Fact]
    public void SelectMany_Lambda()
    {
        var box = "1984".ToBox().SelectMany(content => String2Int(content).ToBox());

        box.Content.Should().Be(1984);
    }

    [Fact]
    public void SelectMany_Empty_Lambda()
    {
        var box = Box<string>.Empty.SelectMany(result1 => String2Int(result1).ToBox());

        box.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void SelectMany_Linq()
    {
        var box =
            from result1 in "1984".ToBox()
            from result2 in String2Int(result1).ToBox()
            select result2;

        box.Content.Should().Be(1984);
    }
}
