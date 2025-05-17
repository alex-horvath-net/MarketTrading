using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Adapters.Blazor;


public record InputModel(
    [Required] string TraderId,
    Guid? Id = null) {


    [Required]
    public string Issuer { get; set; } = "TradingPortal";
}
