using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketDataRelayService.Infrastructure.Repository.Models;

public class MarketPriceEntity {
    [Key]
    public int Id { get; set; }

    public string Symbol { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public double Bid { get; set; }
    public double Ask { get; set; }
    public double Last { get; set; }
    public string CorrelationId { get; set; } = default!;
}
