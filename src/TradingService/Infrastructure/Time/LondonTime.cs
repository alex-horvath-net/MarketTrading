using TradingService.Domain;

namespace TradingService.Infrastructure.Time;
public class LondonTime : ITime {
    public DateTime Now { 
        get {             
            var utcNow = DateTime.UtcNow;
            // London time is UTC+0 or UTC+1 depending on daylight saving time
            var londonNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, londonTimeZone);
            return londonNow;
        }
    }

    private static readonly TimeZoneInfo londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
}
