namespace Core.Sockets.ValidationModel;

public sealed record ValidationSocketModel(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);