using Microsoft.AspNetCore.Mvc;
using Business.Domain;
using Business.Experts.Trader;

namespace OrderManagementService.Controllers;
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
