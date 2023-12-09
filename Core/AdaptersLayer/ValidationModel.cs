namespace Core.AdaptersLayer;

public sealed record ValidationResult(string PropertyName, string ErrorCode, string ErrorMessage, string Severity)
{
}