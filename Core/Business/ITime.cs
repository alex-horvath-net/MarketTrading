namespace Core.Business;
public interface ITime {
  DateTime Now { get; }
  DateTime UtcNow { get; }
}
