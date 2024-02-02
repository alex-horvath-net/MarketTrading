using Core.Business;

namespace Core.Solutions.Time; 
public class MicrosoftTime : ITime {
  public DateTime Now => DateTime.Now;
  public DateTime UtcNow => DateTime.UtcNow; 
}
