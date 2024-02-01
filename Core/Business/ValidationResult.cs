namespace Core.Business;

//public class ValidationResult {
//    public static ValidationResult Success() => new(errorCode: null, errorMessage: null);

//    public static ValidationResult Failed(string errorCode, string errorMessage) {
//        ArgumentException.ThrowIfNullOrWhiteSpace(errorCode);
//        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);
//        return new(errorCode, errorMessage);
//    }

//    public string? ErrorCode { get; }
//    public string? ErrorMessage { get; }
//    public bool IsSuccess => ErrorCode == null && ErrorMessage == null;
//    public bool IsFailed => !IsSuccess;

//    private ValidationResult(string? errorCode = null, string? errorMessage = null) {
//        ErrorCode = errorCode;
//        ErrorMessage = errorMessage;
//    }
//}


public abstract record Result(string? ErrorCode, string? ErrorMessage)
{
    public bool IsSuccess => ErrorCode == null && ErrorMessage == null;
    public bool IsFailed => !IsSuccess;  
}

public record Success() : Result(null, null) 
{
}

public record Failed(string ErrorCode, string ErrorMessage) : Result(ErrorCode, ErrorMessage)
{
} 