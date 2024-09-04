using Common.Business.Model;

namespace Experts.Trader.ReadTransactions.Validate;


public class Adapter(IValidator validator) {
    public async Task<List<Error>> Validate(Request request, CancellationToken token) {
        var dataModel = await validator.Validate(request, token);
        var businessModel = dataModel;
        return businessModel;
    }
}

public interface IValidator {
    public Task<List<string>> Validate(Request request, CancellationToken token);
}



