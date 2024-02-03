using Microsoft.Extensions.Logging;

namespace Core.Business;

public interface ISettings<T> where T : class {
  public T Value { get; }
}