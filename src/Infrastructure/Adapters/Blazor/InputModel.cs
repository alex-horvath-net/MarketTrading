using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Blazor;


public record InputModel {

    [Required]
    public string Issuer { get; set; } = "TradingPortal";
    [Required]
    public string TraderId { get; set; } = "";
}
