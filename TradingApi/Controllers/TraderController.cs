using Domain;
using DomainExperts.Trader;
using Microsoft.AspNetCore.Mvc;

namespace TradingWebApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TraderController(Trader trader) : ControllerBase {

        [HttpGet(template: "transations", Name = "FindAllTransations")]
        public IEnumerable<Trade> Get() {
            return trader.FindAllTransations();
        }
    }
}
