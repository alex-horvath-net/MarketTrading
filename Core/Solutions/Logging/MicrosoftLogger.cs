using Microsoft.Extensions.Logging;

namespace Core.Solutions.Logging;
public class MicrosoftLogger<T>(Microsoft.Extensions.Logging.ILogger<T> logger) : Core.Business.ILogger<T> where T : class
{
    public void Trace(string? template)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template);
    }
    public void Trace<P0>(string template, P0 p0)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0);
    }
    public void Trace<P0, P1>(string template, P0 p0, P1 p1)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1);
    }
    public void Trace<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2);
    }
    public void Trace<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3);
    }
    public void Trace<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3, p4);
    }
    public void Trace<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3, p4, p5);
    }
    public void Trace<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3, p4, p5, p6);
    }
    public void Trace<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3, p4, p5, p6, p7);
    }
    public void Trace<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3, p4, p5, p6, p7, p8);
    }
    public void Trace<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(template, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
    }


    public void Debug(string? template)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template);
    }
    public void Debug<P0>(string template, P0 p0)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0);
    }
    public void Debug<P0, P1>(string template, P0 p0, P1 p1)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1);
    }
    public void Debug<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2);
    }
    public void Debug<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3);
    }
    public void Debug<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3, p4);
    }
    public void Debug<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3, p4, p5);
    }
    public void Debug<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3, p4, p5, p6);
    }
    public void Debug<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3, p4, p5, p6, p7);
    }
    public void Debug<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3, p4, p5, p6, p7, p8);
    }
    public void Debug<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(template, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
    }


    public void Inform(string? template)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template);
    }
    public void Inform<P0>(string template, P0 p0)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0);
    }
    public void Inform<P0, P1>(string template, P0 p0, P1 p1)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1);
    }
    public void Inform<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2);
    }
    public void Inform<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3);
    }
    public void Inform<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3, p4);
    }
    public void Inform<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3, p4, p5);
    }
    public void Inform<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3, p4, p5, p6);
    }
    public void Inform<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3, p4, p5, p6, p7);
    }
    public void Inform<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3, p4, p5, p6, p7, p8);
    }
    public void Inform<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(template, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
    }


    public void Warning(string? template)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template);
    }
    public void Warning<P0>(string template, P0 p0)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0);
    }
    public void Warning<P0, P1>(string template, P0 p0, P1 p1)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1);
    }
    public void Warning<P0, P1, P2>(string template, P0 p0, P1 p1, P2 p2)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2);
    }
    public void Warning<P0, P1, P2, P3>(string template, P0 p0, P1 p1, P2 p2, P3 p3)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3);
    }
    public void Warning<P0, P1, P2, P3, P4>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3, p4);
    }
    public void Warning<P0, P1, P2, P3, P4, P5>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3, p4, p5);
    }
    public void Warning<P0, P1, P2, P3, P4, P5, P6>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3, p4, p5, p6);
    }
    public void Warning<P0, P1, P2, P3, P4, P5, P6, P7>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3, p4, p5, p6, p7);
    }
    public void Warning<P0, P1, P2, P3, P4, P5, P6, P7, P8>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3, p4, p5, p6, p7, p8);
    }
    public void Warning<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(template, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
    }


    public void Error(Exception exception, string template)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template);
    }
    public void Error<P0>(Exception exception, string template, P0 p0)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0);
    }
    public void Error<P0, P1>(Exception exception, string template, P0 p0, P1 p1)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1);
    }
    public void Error<P0, P1, P2>(Exception exception, string template, P0 p0, P1 p1, P2 p2)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2);
    }
    public void Error<P0, P1, P2, P3>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3);
    }
    public void Error<P0, P1, P2, P3, P4>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3, p4);
    }
    public void Error<P0, P1, P2, P3, P4, P5>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3, p4, p5);
    }
    public void Error<P0, P1, P2, P3, P4, P5, P6>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3, p4, p5, p6);
    }
    public void Error<P0, P1, P2, P3, P4, P5, P6, P7>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3, p4, p5, p6, p7);
    }
    public void Error<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3, p4, p5, p6, p7, p8);
    }
    public void Error<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Exception exception, string template, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, template, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
    }
}
