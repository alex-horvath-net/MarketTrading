namespace Core.Business;

public interface ILogger<T> where T : class {
    void LogInformation(string messageTemplate);
    void LogInformation<P0>(string messageTemplate, P0 p0);
    void LogInformation<P0, P1>(string messageTemplate, P0 p0, P1 p1);
    void LogInformation<P0, P1, P2>(string messageTemplate, P0 p0, P1 p1, P2 p2);

    void LogError(Exception exception, string messageTemplate);
    void LogError<P0>(Exception exception, string messageTemplate, P0 p0);
    void LogError<P0, P1>(Exception exception, string messageTemplate, P0 p0, P1 p1);
    void LogError<P0, P1, P2>(Exception exception, string messageTemplate, P0 p0, P1 p1, P2 p2);
}

