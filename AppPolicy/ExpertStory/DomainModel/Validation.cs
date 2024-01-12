namespace Core.ExpertStory.DomainModel;

public class Validation {
    public static Validation Success() => new(errorCode: null, errorMessage: null);

    public static Validation Failed(string errorCode, string errorMessage) {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);
        return new(errorCode, errorMessage);
    }

    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public bool IsSuccess => ErrorCode == null && ErrorMessage == null;

    private Validation(string? errorCode = null, string? errorMessage = null) {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}