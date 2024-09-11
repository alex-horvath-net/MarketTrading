using Common.Validation.FluentValidator.Adapters;
using Experts.Trader.FindTransactions.Validator.Business;

namespace Experts.Trader.FindTransactions.Validator.FluentValidator.Adapter;

public class Adapter(IClient client) : CommonAdapter<Request>(client), IValidator;
