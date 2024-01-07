namespace Core.Sys.Sockets.Validation;

public sealed record ValidationSocketModel(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity); 