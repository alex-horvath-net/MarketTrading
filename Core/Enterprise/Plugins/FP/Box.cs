using FluentAssertions;
using Xunit;

namespace Core.Enterprise.Plugins.FP;

public record Box<T>
{
    private T content;
    public bool IsEmpty { get; private set; }

    public static readonly Box<T> Empty = new() { IsEmpty = true };
    public static Box<T> Create(T content) => new() { content = content, IsEmpty = false };

    private Box() { }

    public static implicit operator Box<T>(T content) => Create(content);
    public static implicit operator T(Box<T> box) => box.content;

    public Box<R> Join<R>()
    {
        if (IsEmpty)
            return Box<R>.Empty;

        if (content is Box<R> boxR)
            return boxR;

        return new Box<R>();
    }

    public Box<R> Select<R>(Func<T, R> mapT2R)
    {
        if (IsEmpty)
            return Box<R>.Empty;

        var t = content;
        var r = mapT2R(t);
        return r;
    }

    public Box<R> SelectMany<R>(Func<T, Box<R>> mapT2BoxR)
    {
        if (IsEmpty)
            return Box<R>.Empty;

        var t = content;
        var boxR = mapT2BoxR(t);
        return boxR;
    }

    public Box<R> SelectMany<U, R>(Func<T, Box<U>> mapT2BoxU, Func<T, U, R> mapTU2R)
    {
        if (IsEmpty)
            return Box<R>.Empty;

        var t = content;
        var boxU = mapT2BoxU(t);
        var u = boxU.content;
        var r = mapTU2R(t, u);
        return r;
    }
}

public static class BoxExtensions
{
    public static Box<T> Box<T>(this T content) => FP.Box<T>.Create(content);
}

public class Box_Design
{
    private int Text2Num(string text) => int.Parse(text);
    private DateTime Num2Date(int year) => new DateTime(year, 1, 1);

    [Fact]
    public void Create_An_Empty_Box()
    {
        var box = Box<string>.Empty;

        box.IsEmpty.Should().BeTrue();
    }
       
    [Fact]
    public void Box_Explicitly_By_Factory_Method()
    {  
        var year = "1984";

        var box = Box<string>.Create(year);

        box.IsEmpty.Should().BeFalse(); 
    }

    [Fact]
    public void Box_Explicitly_By_Extension_Method()
    {
        var year = "1984";

        var box = year.Box(); 

        box.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Box_An_Other_Box()
    {
        var year = "1984";  
        var box = year.Box(); 

        var newBox = box.Box();

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
        var box = year.Box();

        string content = box;

        content.Should().Be(year);
    }

    [Fact]
    public void Rebox_An_Empty_Old_Box_By_Lambda_Select()
    {
        var oldBox = Box<string>.Empty;

        var newBox = oldBox.Select(Text2Num);

        newBox.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Rebox_A_Non_Empty_Old_Box_By_Lambda_Select()
    { 
        var year = "1984";  
        var oldBox = year.Box();

        var newBox = oldBox.Select(Text2Num);
                 
        newBox.IsEmpty.Should().BeFalse();
        newBox.Should().Be(1984.Box());
    }
   
    [Fact]
    public void Rebox_Old_Box_By_Linq_Select()
    {
        var year = "1984";
        var oldBox = year.Box(); 

        var newBox =
            from content in oldBox
            select Text2Num(content);

        newBox.IsEmpty.Should().BeFalse();
        newBox.Should().Be(1984.Box());
    }
    [Fact]
    public void Rebox_An_Old_Box_Without_Any_Change_By_Lambda_Select_Functor_1()
    {
        var year = "1984"; 
        var oldBox = year.Box();

        var newBox = oldBox.Select(content => content); 

        newBox.IsEmpty.Should().BeFalse();
        newBox.Should().Be(year.Box()); 
        newBox.Should().Be(oldBox);
    }
    [Fact]
    public void Rebox_An_Old_Box_Wit_2_Changes_By_Lambda_Select_Functor_2()
    {
        var year = "1984";
        var date  = DateTime.Parse($"{year}.01.01");   
        var oldBox = year.Box(); 

        var newBoxBy2Select1Map = oldBox.Select(Text2Num).Select(Num2Date);
        var newBoxBy1Select2Map = oldBox.Select(content => Num2Date(Text2Num(content)));

        newBoxBy2Select1Map.IsEmpty.Should().BeFalse();
        newBoxBy2Select1Map.Should().Be(date.Box());
        newBoxBy1Select2Map.IsEmpty.Should().BeFalse();
        newBoxBy1Select2Map.Should().Be(date.Box());
        newBoxBy2Select1Map.Should().Be(newBoxBy1Select2Map);
    }

    [Fact]
    public void Flat_Non_Nested_Empty_Box_To_Right_Type()
    {
        var box = Box<string>.Empty;
        
        var flatBox = box.Join<string>();

        flatBox.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Flat_Nested_Empty_Box_To_Right_Type()
    { 
        var box = Box<string>.Empty;
        var boxInBox = box.Box();

        var flatBox = boxInBox.Join<string>();

        flatBox.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Flat_Nested_Non_Empty_Box_To_Wrong_Type()
    {
        var box = "1984".Box();
        var boxInBox = box.Box();

        var flatBox = boxInBox.Join<int>();

        flatBox.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Flat_Nested_Non_Empty_Box_To_Right_Type()
    {
        var box = "1984".Box();
        var boxInBox = box.Box();
         
        var flatBox = boxInBox.Join<string>();

        flatBox.Should().Be(box);
    }

    [Fact]
    public void SelectMany_Lambda_1()
    {
        var box = "1984".Box().SelectMany(content => Text2Num(content).Box());

        box.Should().Be(1984.Box());
    }

    [Fact]
    public void SelectMany_Lambda_2()
    {
        var box = Box<string>.Empty.SelectMany<Int32,DateTime>(text => Text2Num(text).Box(), (text, num)=> Num2Date(num));

        box.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void SelectMany_Empty_Lambda()
    {
        var box = Box<string>.Empty.SelectMany(result1 => Text2Num(result1).Box());

        box.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void SelectMany_Linq()
    {
        var box =
            from step1 in "1984".Box()
            from step2 in Text2Num(step1).Box()
            select step2;

        box.Should().Be(1984.Box());
    }
}
