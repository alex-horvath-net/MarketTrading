using FluentAssertions;
using Xunit;

namespace Core.Enterprise.Plugins.FP;

public class BoxDesign
{
    private int Text2Num(string text) => int.Parse(text);
    private DateTime Num2Date(int year) => new DateTime(year, 1, 1);

    [Fact]
    public void Create_An_Empty_Box()
    {
        var box = new Box<string>();

        box.IsEmpty.Should().BeTrue();
        box.Should().BeOfType<Box<string>>();
    }

    [Fact]
    public void Box_Explicitly_By_Factory_Method()
    {
        var year = "1984";

        var box = new Box<string>(year);

        box.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Box_Explicitly_By_Extension_Method()
    {
        var year = "1984";

        var box = year.ToBox();

        box.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Box_An_Other_Box()
    {
        var year = "1984";
        var box = year.ToBox();

        var newBox = box.ToBox();

        newBox.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Box_Implicitly()
    {
        var year = "1984";

        Box<string> box = year;

        box.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Unbox_Implicitly()
    {
        var year = "1984";
        var box = year.ToBox();

        string content = box;

        content.Should().Be(year);
    }

    [Fact]
    public void Rebox_An_Empty_Box_By_Lambda_Select()
    {
        var oldBox = new Box<string>();

        var newBox = oldBox.Select(Text2Num);

        newBox.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Rebox_An_Old_Box_By_Select_Lambda()
    {
        var year = "1984";
        var oldBox = year.ToBox();

        var newBox = oldBox.Select(Text2Num);

        newBox.IsEmpty.Should().BeFalse();
        newBox.Should().Be(1984.ToBox());
    }

    [Fact]
    public void Rebox_Old_Box_By_Linq_Select()
    {
        var year = "1984";
        var oldBox = year.ToBox();

        var newBox =
            from content in oldBox
            select Text2Num(content);

        newBox.IsEmpty.Should().BeFalse();
        newBox.Should().Be(1984.ToBox());
    }
    [Fact]
    public void Rebox_An_Old_Box_Without_Any_Change_By_Lambda_Select_Functor_1()
    {
        var year = "1984";
        var oldBox = year.ToBox();

        var newBox = oldBox.Select(content => content);

        newBox.IsEmpty.Should().BeFalse();
        newBox.Should().Be(year.ToBox());
        newBox.Should().Be(oldBox);
    }
    [Fact]
    public void Rebox_An_Old_Box_Wit_2_Changes_By_Lambda_Select_Functor_2()
    {
        var year = "1984";
        var date = DateTime.Parse($"{year}.01.01");
        var oldBox = year.ToBox();

        var newBoxBy2Select1Map = oldBox.Select(Text2Num).Select(Num2Date);
        var newBoxBy1Select2Map = oldBox.Select(content => Num2Date(Text2Num(content)));

        newBoxBy2Select1Map.IsEmpty.Should().BeFalse();
        newBoxBy2Select1Map.Should().Be(date.ToBox());
        newBoxBy1Select2Map.IsEmpty.Should().BeFalse();
        newBoxBy1Select2Map.Should().Be(date.ToBox());
        newBoxBy2Select1Map.Should().Be(newBoxBy1Select2Map);
    }

    [Fact]
    public void Flat_Nested_Empty_Box_To_Right_Type()
    {
        var box = new Box<string>();
        var boxInBox = box.ToBox();

        var flatBox = boxInBox.Join();

        flatBox.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Flat_Nested_Non_Empty_Box_To_Wrong_Type()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        var flatBox = boxInBox.Join();

        flatBox.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Flat_Nested_Non_Empty_Box_To_Right_Type()
    {
        var box = "1984".ToBox();
        var boxInBox = box.ToBox();

        var flatBox = boxInBox.Join();

        flatBox.Should().Be(box);
    }

    [Fact]
    public void SelectMany_Lambda_1()
    {
        var box = "1984".ToBox().SelectMany(content => Text2Num(content).ToBox());

        box.Should().Be(1984.ToBox());
    }

    [Fact]
    public void SelectMany_Lambda_2()
    {
        var box = new Box<string>().SelectMany(text => Text2Num(text).ToBox(), (text, num) => Num2Date(num));

        box.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void SelectMany_Empty_Lambda()
    {
        var box = new Box<string>().SelectMany(result1 => Text2Num(result1).ToBox());

        box.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void SelectMany_Linq()
    {
        var box =
            from step1 in "1984".ToBox()
            from step2 in Text2Num(step1).ToBox()
            select step2;

        box.Should().Be(1984.ToBox());
    }
}
