using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Client.Trader.FindTrades;
public class FindTradesClient(HttpClient http) : IFindTradesClient {

    public FindTradesInputModel InputModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public FindTradesViewModel ViewModel => throw new NotImplementedException();

    public Task Execute(CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }
}
