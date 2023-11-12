using NSubstitute;
using Xunit.Sdk;

namespace HiringManager.ReadJobRoles.Business
{
    public interface IValidator
    {
        Task<Result> Validate(Request request, CancellationToken token);

        public static class Mock
        {
            public static IValidator Simple()
            {
                var mock = Substitute.For<IValidator>();
                mock.Validate(default, default).ReturnsForAnyArgs(Result.Success());
                return mock;
            }
        }
    }

    public sealed record Error(string Code, string Message = null)
    {
        public static readonly Error None = new Error(string.Empty);
        public static implicit operator Result(Error error)=> Result.Failure(error);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);

        private Result(bool success, Error error)
        {
            if (success && error != Error.None || !success && error == Error.None) throw new ArgumentException("Invalied error", nameof(error));
            IsSuccess = success;
            Error = error;
        }
    }
}