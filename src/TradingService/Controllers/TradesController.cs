using Microsoft.AspNetCore.Mvc;
using Domain;

namespace TradingService.Controllers;
[ApiController]
[Route("api/trades")]
public class TradesController(Trader trader) : ControllerBase {

    [HttpGet("transations")]
    public async Task<IEnumerable<Trade>> Get() {
        //return await trader.FindTrades();
        return default;
    }

    [HttpHead("ping")]
    public void Head() {
    }
}
 