namespace Core.Sys.Sockets.ValidationModel;

public sealed record ValidationSocketModel(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);