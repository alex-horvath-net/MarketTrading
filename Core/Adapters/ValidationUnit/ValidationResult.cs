namespace Core.Adapters.ValidationUnit;

public sealed record ValidationResult(string PropertyName, string ErrorCode, string ErrorMessage, string Severity);