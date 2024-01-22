namespace Core.Solutions.Validation.Model;

public record ValidationIssue(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);