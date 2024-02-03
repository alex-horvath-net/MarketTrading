using Core.Business;
using Microsoft.Extensions.Options;

namespace Core.Solutions.Setting;
public class MicrosoftSettings<T> : ISettings<T> where T : class {
  public MicrosoftSettings(IOptionsMonitor<T> options) {
    Value = options.CurrentValue;
    options.OnChange(updatedValue => Value = updatedValue);
  }
  public T Value { get; private set; }
}
