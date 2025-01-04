using BusinesActors.Trader;
using BusinessDomain;
using Microsoft.AspNetCore.Mvc;

namespace TradingWebApi.Controllers; 
[ApiController]
[Route("[controller]")]
public class TraderController(Trader trader) : ControllerBase {

    [HttpGet("transations")]
    public IEnumerable<Trade> Get() {
        return trader.FindAllTransations();
    }

    [HttpHead("ping")]
    public void Head() {
    }
}
