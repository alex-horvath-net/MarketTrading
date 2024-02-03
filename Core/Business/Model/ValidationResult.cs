namespace Core.Business.Model;

public abstract record Result(string? ErrorCode, string? ErrorMessage) {
  public bool IsSuccess => ErrorCode == null && ErrorMessage == null;
  public bool IsFailed => !IsSuccess;
}

public record Success() : Result(null, null);

public record Failed(string ErrorCode, string ErrorMessage) : Result(
    ErrorCode ?? throw new ArgumentException(nameof(ErrorCode)),
    ErrorMessage ?? throw new ArgumentException(nameof(ErrorMessage)));