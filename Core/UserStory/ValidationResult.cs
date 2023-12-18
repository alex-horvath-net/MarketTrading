using FluentAssertions;
using Xunit;

namespace Core.UserStory;

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

public class ValidationResult_Design
{
    [Fact]
    public void ValidationResult_Success()
    {
        var result = ValidationResult.Success();

        result.Should().NotBeNull();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ValidationResult_Failed()
    {
        var result = ValidationResult.Failed("ErrorCode", "ErrorMessage");

        result.Should().NotBeNull();
        result.ErrorCode.Should().NotBeNull();
        result.ErrorMessage.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}
