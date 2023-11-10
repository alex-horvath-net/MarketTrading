using System.Linq.Expressions;
using HiringManager.ReadJobRoles.Business;
using Shared;
using Shared.Business;

namespace HiringManager.ReadJobRoles.Adapters
{
    public class Test
    {
        public IRepository GetRepositoryMock()
        {
            var repository = new Builder<IRepository>()
                .With(x => x.Read(default, default), new List<JobRole>())
                .Build();
            return repository;
        }

    }

    public class Builder<T> where T : class
    {
        public Builder<T> With<R>(Expression<Func<T, Task<R>>> expression, R returnValue)
        {
            var key = expression.ToString();
            if (!map.ContainsKey(key))
                map.Add(key, returnValue);

            return this;
        }

        public T Build()
        {
            return default;
        }

        Dictionary<string, object> map = new();
    }

    public class Mock
    {
        public static HalfProxy Me<T>()
        {
            return new HalfProxy();
        }

    }

    public class HalfProxy
    {
        public HalfProxy SomeProperty
        {
            get
            {
                return this;
            }
        }
        public HalfProxy Read(Request request, CancellationToken token)
        {

            return this;
        }


        public HalfProxy Returns<T>(T value)
        {
            return this;
        }

        public HalfProxy Throws<T>(Exception exception) where T : Exception
        {
            throw exception;
        }

    }

    public class Sample<T> where T : class
    {
        public static implicit operator T(Sample<T> mock) => default(T);
        public static implicit operator Sample<T>(T value) => new Sample<T>();
    }
}
