//using FluentAssertions;
//using Principals.UserStoryCore.UserStoryUnit;
//using Sys.UserStoryCore.UserStoryUnit;
//using Xunit;

//namespace Core.UserStoryCore.UserStoryUnit;

////public class ValidationResult
////{
////    public bool IsSuccess { get; }
////    public bool IsFailure => !IsSuccess;
////    public ValidationResult ValidationResult { get; }

////    public static ValidationResult Success() => new ValidationResult(true, ValidationResult.None);
////    public static ValidationResult Failure(ValidationResult error) => new ValidationResult(false, error);

////    private ValidationResult(bool success, ValidationResult error)
////    {
////        if (success && error != ValidationResult.None || !success && error == ValidationResult.None) throw new ArgumentException("Invalied error", nameof(error));
////        IsSuccess = success;
////        ValidationResult = error;
////    }
////}

////--Specification--------------------------------------------------


//public class UserStory_Spec
//{
//    [Fact]
//    public async void NonStoppedFeature()
//    {
//        var tasks = new List<ITask<Response<Request>>>()
//        {
//            task.Stopped().Mock,
//            task.NonStopped().Mock
//        };

//        var unit = new UserStoryCore<Request, Response<Request>>(tasks);
//        var response = await unit.Run(feature.Request, feature.Token);

//        response.Should().NotBeNull();
//        response.Request.Should().Be(feature.Request);
//        response.Stopped.Should().BeFalse();
//        response.Validations.Should().BeNull();
//    }

//    [Fact]
//    public async void StoppedFeature()
//    {
//        workSteps.MockStoppedWorkSteps();

//        var unit = new UserStoryCore<Request, Response<Request>>(workSteps.Mock);
//        var response = await unit.Run(feature.Request, feature.Token);

//        response.Should().NotBeNull();
//        response.Request.Should().Be(feature.Request);
//        response.Stopped.Should().BeTrue();
//    }

//    private readonly ITask<Response<Request>>.TaskMockBuilder task = new();
//    private readonly Featrue_MockBuilder feature = new();
//}
