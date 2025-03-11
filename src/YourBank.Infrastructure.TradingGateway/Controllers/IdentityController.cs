using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;
[ApiController]
[Route("[controller]")]
public class IdentityController() : ControllerBase {

  
    [HttpHead("ping")]
    public void Head() {
    }
}
