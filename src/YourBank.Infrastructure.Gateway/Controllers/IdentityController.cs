using Microsoft.AspNetCore.Mvc;
using YourBank.Business.Domain;
using YourBank.Business.Experts.Trader;

namespace YourBank.Infrastructure.TradingGateway.Controllers;
[ApiController]
[Route("[controller]")]
public class IdentityController(Trader trader) : ControllerBase {

    [HttpGet("transations")]
    public IEnumerable<Trade> Get() {
        return trader.FindAllTransations();
    }
     
    [HttpHead("ping")]
    public void Head() {
    }
}
