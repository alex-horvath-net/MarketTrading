using Microsoft.Extensions.Logging;

namespace Core.Solutions.Logging;
public class MicrosoftLogger<T>(Microsoft.Extensions.Logging.ILogger<T> logger) : Core.Business.ILogger<T> where T : class {
    public void LogInformation(string messageTemplate) {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(messageTemplate);
    }

    public void LogInformation<P0>(string messageTemplate, P0 p0) {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(messageTemplate, p0);
    }

    public void LogInformation<P0, P1>(string messageTemplate, P0 p0, P1 p1) {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(messageTemplate, p0, p1);
    }

    public void LogInformation<P0, P1, P2>(string messageTemplate, P0 p0, P1 p1, P2 p2) {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(messageTemplate, p0, p1, p2);
    }

    public void LogError(Exception exception, string messageTemplate) {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, messageTemplate);
    }

    public void LogError<P0>(Exception exception, string messageTemplate, P0 p0) {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, messageTemplate, p0);
    }

    public void LogError<P0, P1>(Exception exception, string messageTemplate, P0 p0, P1 p1) {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, messageTemplate, p0, p1);
    }

    public void LogError<P0, P1, P2>(Exception exception, string messageTemplate, P0 p0, P1 p1, P2 p2) {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, messageTemplate, p0, p1, p2);
    }
}
