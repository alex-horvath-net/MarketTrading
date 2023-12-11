namespace Core.Sockets.Validation;

public sealed record ValidationResult(string PropertyName, string ErrorCode, string ErrorMessage, string Severity);