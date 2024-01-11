namespace Core.UserStory.DomainModel;

public sealed class ValidationDomainModel {
    public static ValidationDomainModel Success() => new(errorCode: null, errorMessage: null);

    public static ValidationDomainModel Failed(string errorCode, string errorMessage) {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);
        return new(errorCode, errorMessage);
    }

    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public bool IsSuccess => ErrorCode == null && ErrorMessage == null;

    private ValidationDomainModel(string? errorCode = null, string? errorMessage = null) {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}