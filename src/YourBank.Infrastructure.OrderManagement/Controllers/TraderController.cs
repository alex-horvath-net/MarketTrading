using Microsoft.AspNetCore.Mvc;
using Business.Domain;
using Business.Experts.Trader;

namespace TradingService.Controllers;
[ApiController]
[Route("[controller]")]
public class TraderController(Trader trader) : ControllerBase {

    [HttpGet("transations")]
    public async Task<IEnumerable<Trade>> Get() {
        //return await trader.FindTrades();
        return default;
    }

    [HttpHead("ping")]
    public void Head() {
    }
}
