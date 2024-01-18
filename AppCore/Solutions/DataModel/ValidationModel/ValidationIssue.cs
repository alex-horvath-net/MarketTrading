namespace Common.Solutions.DataModel.ValidationModel;

public record ValidationIssue(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);