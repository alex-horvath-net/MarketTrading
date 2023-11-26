namespace Core.Business;

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