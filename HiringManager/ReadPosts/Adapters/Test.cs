//using System.Linq.Expressions;
//using HiringManager.ReadJobRoles.Business;
//using Shared.Business;

//namespace HiringManager.ReadJobRoles.Adapters
//{
//    public class Specify
//    {
//        public static IRepository CreateRepositoryDeafaultMock()
//        {
//            var Mock = new MockBuilder<IRepository, List<Post>>();

//            var featureRequest = new Request("TestName");
//            var token = CancellationToken.None;
//            var jobRoles = new List<Post> { new Post("TestName1"), new Post("TestName2") };
//            Mock.Call(x => x.Read(featureRequest, token), jobRoles);

//            return Mock.Build();
//        }
//    }

//    public class MockBuilder<T, R>
//    {
//        public MockBuilder<T, R> Call(Expression<Func<T, R>> key, R value)
//        {
//            if (!map.ContainsKey(key)) map.Add(key, default);
//            map[key] = () => value;
//            return this;
//        }

//        public MockBuilder<T, R> Call<E>(Expression<Func<T, R>> key, E exception = null) where E : Exception
//        {
//            if (!map.ContainsKey(key)) map.Add(key, default);
//            map[key] = () => throw exception;
//            return this;
//        }

//        public MockBuilder<T, R> Call(Expression<Func<T, Task<R>>> key, R value)
//        {
//            if (!mapAsync.ContainsKey(key)) mapAsync.Add(key, default);
//            mapAsync[key] = () => value;
//            return this;
//        }

//        public MockBuilder<T, R> Call<E>(Expression<Func<T, Task<R>>> key, E exception = null) where E : Exception
//        {
//            if (!mapAsync.ContainsKey(key)) mapAsync.Add(key, default);
//            mapAsync[key] = () => throw exception;
//            return this;
//        }


//        public T Build()
//        {
//            return default;
//        }

//        Dictionary<Expression<Func<T, Task<R>>>, Func<R>> mapAsync = new();
//        Dictionary<Expression<Func<T, R>>, Func<R>> map = new();
//    }

//    public record Call<T, R>(Expression<Func<T, Task<R>>> Expression, R ReturnValue)
//    {
//    }


//    /////////////////////////////////

//    public class Mock2
//    {
//        public static HalfProxy Me<T>()
//        {
//            return new HalfProxy();
//        }

//    }

//    public class HalfProxy
//    {
//        public HalfProxy SomeProperty
//        {
//            get
//            {
//                return this;
//            }
//        }
//        public HalfProxy Read(Request request, CancellationToken token)
//        {

//            return this;
//        }


//        public HalfProxy Returns<T>(T value)
//        {
//            return this;
//        }

//        public HalfProxy Throws<T>(Exception exception) where T : Exception
//        {
//            throw exception;
//        }

//    }

//    public class Sample<T> where T : class
//    {
//        public static implicit operator T(Sample<T> Mock) => default(T);
//        public static implicit operator Sample<T>(T value) => new Sample<T>();
//    }
//}
