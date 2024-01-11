namespace Common.Models.ValidationModel;

public record Validation(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);