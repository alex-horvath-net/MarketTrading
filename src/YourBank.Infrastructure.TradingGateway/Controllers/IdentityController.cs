using Microsoft.AspNetCore.Mvc;

namespace YourBank.Infrastructure.TradingGateway.Controllers;
[ApiController]
[Route("[controller]")]
public class IdentityController() : ControllerBase {

  
    [HttpHead("ping")]
    public void Head() {
    }
}
