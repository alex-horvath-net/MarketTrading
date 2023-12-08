using Sys.Plugins.FP;

namespace Spec.Core_Specification.Plugins.FP;

public class Specify_Box
{
    private string MapToItself(string itself) => itself;
    private int Parse(string text) => int.Parse(text);
    private DateTime ToDate(int year) => new DateTime(year, 1, 1);

    //[Fact]
    public void ToBox()
    {
        var box = "1984".ToBox();

        box.Content.Should().Be("1984");
    }

    //[Fact]
    public void Select_Lambda()
    {
        var box1 = "1984".ToBox();

        var box2 = box1.Select(Parse);

        box2.Content.Should().Be(1984);
    }
    //[Fact]
    public void Select_Linq()
    {
        var box1 = "1984".ToBox();

        var box2 =
            from result1 in box1
            select Parse(result1);

        box2.Content.Should().Be(1984);
    }
    //[Fact]
    public void Map_Functor_1()
    {
        var box = "1984".ToBox();
        var newBox = box.Select(MapToItself);

        newBox.Should().Be(box);
    }
    //[Fact]
    public void Map_Functor_2()
    {
        var box = "1984".ToBox();
        var sequentialBox = box.Select(Parse).Select(ToDate);
        var nestedBox = box.Select(c => ToDate(Parse(c)));

        sequentialBox.Should().Be(nestedBox);
    }


    //[Fact]
    public void Join()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        boxInBox.Content.Should().Be(box);
        boxInBox.Join().Should().Be(box);
    }

    //[Fact]
    public void SelectMany_Lambda()
    {
        var box = "1984".ToBox().SelectMany(result1 => Parse(result1).ToBox());

        box.Content.Should().Be(1984);
    }

    //[Fact]
    public void SelectMany_Linq()
    {
        var box =
            from result1 in "1984".ToBox()
            from result2 in Parse(result1).ToBox()
            select result2;

        box.Content.Should().Be(1984);
    }
}
