namespace Core.Sockets.ValidationModel;

public sealed record ValidationSolutionExpertModel(
    string PropertyName,
    string ErrorCode,
    string ErrorMessage,
    string Severity);