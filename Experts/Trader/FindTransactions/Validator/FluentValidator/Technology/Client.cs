using Common.Validation.FluentValidator.Technology;
using Experts.Trader.FindTransactions.Validator.FluentValidator.Adapter;
using FluentValidation;

namespace Experts.Trader.FindTransactions.Validator.FluentValidator.Technology;

public class Client(IValidator<Request> validator) : CommonClient<Request>(validator), IClient;
