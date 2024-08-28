namespace Experts.Trader.ReadTransactions.Validate;


public class ValidatorAdapterPlugin(ValidatorAdapterPlugin.ValidatorTechnologyPort validatorTechnologyPort) : Feature.IValidatorAdapterPort {
    public async Task<List<string>> Validate(Request request, CancellationToken token) {
        var adapterData = await validatorTechnologyPort.Validate(request, token);
        var businessData = adapterData;
        return businessData;
    }


    private Common.Business.Transaction ToDomain(Common.Adapters.AppDataModel.Transaction data) => new() {
        Id = data.Id
    };

    public interface ValidatorTechnologyPort {
        public Task<List<string>> Validate(Request request, CancellationToken token);
    }
}


