using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Blazor;


public record InputModel(
    [Required] string TraderId ) {

    [Required]
    public string Issuer { get; set; } = "TradingPortal";
}
