namespace Experts.Trader.ReadTransactions.Business;

public class Feature(
    IValidatorAdapterPort validator,
    IRepositoryAdapterPort repository)
{
    public async Task<Response> Execute(Request request, CancellationToken token)
    {
        var response = new Response();

        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await repository.ReadTransaction(request, token);

        return response;
    }
}
