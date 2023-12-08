using Sys.Plugins.FP;

namespace Spec.Core_Specification.Plugins.FP
{
    public class Specify_Functor
    {
        //[Fact]
        public void Maping_To_Itself_Makes_No_Change_With_Lambda()
        {
            var sut = new Functor<int>(42);
            var result = sut.Select(x => x);
            sut.Should().Be(result);
        }

        //[Fact]
        public void Maping_To_Itself_Makes_No_Change_With_Linq()
        {
            var sut = new Functor<int>(42);
            var result =
                from x in sut
                select x;
            sut.Should().Be(result);
        }

        //[Fact]
        public void Sequintial_And_Nested_Maping_Are_Identical_With_Lambda()
        {
            Func<int, string> map1 = i => i.ToString();
            Func<string, string> map2 = s => new string(s.Reverse().ToArray());
            var sut = new Functor<int>(42);
            var result1 = sut.Select(map1).Select(map2);
            var result2 = sut.Select(x => map2(map1(x)));
            sut.Should().NotBe(result1);
            result1.Should().Be(result2);
        }

        public void Sequintial_And_Nested_Maping_Are_Identical_With_Linq()
        {
            Func<int, string> map1 = i => i.ToString();
            Func<string, string> map2 = s => new string(s.Reverse().ToArray());
            var sut = new Functor<int>(42);
            var result1 = sut.Select(map1).Select(map2);
            var result2 = sut.Select(x => map2(map1(x)));
            sut.Should().NotBe(result1);
            result1.Should().Be(result2);
        }
    }
}
