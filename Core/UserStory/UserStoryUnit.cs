using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sys.UserStory;

public interface IUserStory<TRequest, TResponse>
{
    Task<TResponse> Run(TRequest request, CancellationToken cancellation);
}

public interface ITask<TResponse> where TResponse : Response<Request>
{
    Task Run(TResponse response, CancellationToken cancellation);

    public class MockBuilder
    {
        public ITask<Response<Request>> Mock = Substitute.For<ITask<Response<Request>>>();

        public MockBuilder Stopped()
        {
            Mock.WhenForAnyArgs(x => x.Run(default, default)).Do(x => { });
            return this;
        }

        public MockBuilder NonStopped()
        {
            Mock.WhenForAnyArgs(x => x.Run(default, default)).Do(x => { });
            return this;
        }
    }
}

public class UserStory<TRequest, TResponse>(IEnumerable<ITask<TResponse>> workSteps)
         : IUserStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken cancellation)
    {
        var response = new TResponse() with { Request = request };
        foreach (var workStep in workSteps)
        {
            await workStep.Run(response, cancellation);
            cancellation.ThrowIfCancellationRequested();
            if (response.Stopped) return response;
        }
        return response;
    }
}

public record Request();

public record Response<TRequest>() where TRequest : Request
{
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
    public bool Stopped { get; set; }
}

public sealed class ValidationResult
{
    public static ValidationResult Success() => new(errorCode: null, errorMessage: null);

    public static ValidationResult Failed(string errorCode, string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);
        return new(errorCode, errorMessage);
    }

    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public bool IsSuccess => ErrorCode == null && ErrorMessage == null;

    private ValidationResult(string? errorCode = null, string? errorMessage = null)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}


//public class ValidationResult
//{
//    public bool IsSuccess { get; }
//    public bool IsFailure => !IsSuccess;
//    public ValidationResult ValidationResult { get; }

//    public static ValidationResult Success() => new ValidationResult(true, ValidationResult.None);
//    public static ValidationResult Failure(ValidationResult error) => new ValidationResult(false, error);

//    private ValidationResult(bool success, ValidationResult error)
//    {
//        if (success && error != ValidationResult.None || !success && error == ValidationResult.None) throw new ArgumentException("Invalied error", nameof(error));
//        IsSuccess = success;
//        ValidationResult = error;
//    }
//}

//--Specification--------------------------------------------------


public class UserStory_Spec
{
    [Fact]
    public async void NonStoppedFeature()
    {
        var tasks = new List<ITask<Response<Request>>>()
        {
            task.Stopped().Mock,
            task.NonStopped().Mock
        };

        var unit = new UserStory<Request, Response<Request>>(tasks);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.Stopped.Should().BeFalse();
        response.Validations.Should().BeNull();
    }

    [Fact]
    public async void StoppedFeature()
    {
        workSteps.MockStoppedWorkSteps();

        var unit = new UserStory<Request, Response<Request>>(workSteps.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.Stopped.Should().BeTrue();
    }

    private readonly ITask<Response<Request>>.MockBuilder task = new();
    private readonly Featrue_MockBuilder feature = new();
}

public class Featrue_MockBuilde qaszr
{
    public readonly IUserStory<Request, Response> Mock = Substitute.For<IUserStory<Request, Response>>();
    public Request Request;
    public CancellationToken Token;

    public Featrue_MockBuilder() => UseValidRequest().UseNoneCanceledToken();

    public Featrue_MockBuilder UseValidRequest()
    {
        Request = new Request("Title", "Content");
        Request = Request with { Title = Request.Title, Content = Request.Content };
        return this;
    }

    public Featrue_MockBuilder UseInvalidRequest()
    {
        Request = new Request(null, null);
        Request = Request with { Title = Request.Title, Content = Request.Content };
        return this;
    }

    public Featrue_MockBuilder UseNoneCanceledToken()
    {
        Token = CancellationToken.None;
        return this;
    }
}

public class Tasks_MockBuilder
{
    public readonly List<ITask<Response>> Mock = new List<ITask<Response>>();

    public Tasks_MockBuilder() => UseNonStoppedWorkSteps();

    public Tasks_MockBuilder UseNonStoppedWorkSteps()
    {
        Mock.Clear();
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new ContinueWorkStep());
        return this;
    }

    public Tasks_MockBuilder MockStoppedWorkSteps()
    {
        Mock.Clear();
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new StopWorkStep());
        Mock.Add(new ContinueWorkStep());
        return this;
    }

    public class StopWorkStep : ITask<Response>
    {
        public Task Run(Response response, CancellationToken cancellation)
        {
            response.Stopped = true;
            return Task.CompletedTask;
        }
    }

    public class ContinueWorkStep : ITask<Response>
    {
        public Task Run(Response response, CancellationToken cancellation)
        {
            response.Stopped = false;
            return Task.CompletedTask;
        }
    }
}