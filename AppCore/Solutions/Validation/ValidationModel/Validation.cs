namespace Common.Solutions.Validation.ValidationModel;

public record ValidationIssue(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);