namespace DomainExperts.Trader.FindTransactions.UserStory.InputPort;

public interface IUserStory {
    Task<Response> Execute(Request request, CancellationToken token);
}