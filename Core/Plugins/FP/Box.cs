using FluentAssertions;
using Xunit;

namespace Core.Plugins.FP;

public record Box<T>
{
    public T Content { get; private set; }
    public bool HasContent { get; private set; }

    public static readonly Box<T> Empty= new();
    public static Box<T> New(T content) => new() { Content = content, HasContent = true };

    private Box() { }

    public static implicit operator Box<T>(T content) => New(content);
    public static implicit operator T(Box<T> box) => box.Content;

    public Box<R> Join<R>()
    {
        if (!HasContent)
            return Box<R>.Empty;

        if (Content is Box<R> boxR)
            return boxR;

        return new Box<R>();
    }

    public Box<R> Select<R>(Func<T, R> mapT2R)
    {
        if (!HasContent)
            return Box<R>.Empty;

        var t = Content;
        var r = mapT2R(t);
        return r;
    }

    public Box<R> SelectMany<R>(Func<T, Box<R>> mapT2BoxR)
    {
        if (!HasContent)
            return Box<R>.Empty;

        var t = Content;
        var boxR = mapT2BoxR(t);
        return boxR;
    }

    public Box<R> SelectMany<U, R>(Func<T, Box<U>> mapT2BoxU, Func<T, U, R> mapTU2R)
    {
        if (!HasContent)
            return Box<R>.Empty;

        var t = Content;
        var boxU = mapT2BoxU(t);
        var u = boxU.Content;
        var r = mapTU2R(t, u);
        return r;
    }


}

public static class BoxExtensions
{
    public static Box<T> ToBox<T>(this T content) => Box<T>.New(content);

}

public class Box_Design
{
    private int String2Int(string text) => int.Parse(text);
    private DateTime Int2Date(int year) => new DateTime(year, 1, 1);

    [Fact]
    public void EmptyBox()
    {
        Box<string>.Empty.Should().Be(Box<string>.Empty);
    }

    [Fact]
    public void CreateExplicitEmptyBox()
    {
        var defaultValue = default(string);
        
        var box = Box<string>.Empty;

        box.HasContent.Should().BeFalse();
        box.Content.Should().Be(defaultValue);
    }

    [Fact]
    public void CreatExpliciteNonEmptyBox()
    {
        var year = "1984";  

        var box = Box<string>.New(year);

        box.HasContent.Should().BeTrue();
        box.Content.Should().Be(year);
    }

    [Fact]
    public void CreateImplicitNonEmptyBox()
    {
        var year = "1984";

        Box<string> box = year;

        box.HasContent.Should().BeTrue();
        box.Content.Should().Be("1984");
    }

    [Fact]
    public void GetImplicitContentOfEmptyBox()
    {
        var year = "1984";

        string content = year.ToBox();

        content.Should().Be(year);
    }

    [Fact]
    public void ToBox()
    {
        var year = "1984";

        var box = year.ToBox();

        box.Content.Should().Be("1984");
    }

    [Fact]
    public void Select_Lambda()
    {
        var box = "1984".ToBox();
            
        var reBox = box.Select(String2Int);

        box.Content.Should().Be("1984");
    }
    [Fact]
    public void Select_Empty_Lambda()
    {
        var emptyBox = Box<string>.Empty;   

        var box = emptyBox.Select(String2Int);

        box.HasContent.Should().BeFalse();
    }
    [Fact]
    public void Select_Linq()
    {
        var box = "1984".ToBox();

        var reBox =
            from content in box
            select String2Int(content);

        reBox.Content.Should().Be(1984);
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
    public void Nested_Boxe()
    {
        var box = "1984".ToBox();
        
        var boxInBox = box.ToBox();

        boxInBox.Content.Should().Be(box);
    }


    [Fact]
    public void Join_WorngBox()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        var flatBox = boxInBox.Join<int>();
        
        flatBox.HasContent.Should().BeFalse();
    }

    [Fact]
    public void Join_RightButEmptyBox()
    {
        var box = Box<string>.Empty;
        var boxInBox = box.ToBox();

        var flatBox = boxInBox.Join<string>();

        flatBox.HasContent.Should().BeFalse();
    }


    [Fact]
    public void Join_RightBox()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        var flatBox = boxInBox.Join<string>();
        
        flatBox.Should().Be(box);
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

        box.HasContent.Should().BeFalse();
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
