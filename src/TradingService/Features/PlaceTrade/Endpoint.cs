namespace TradingService.Features.PlaceTrade;

public static class Endpoint {
    public static IEndpointRouteBuilder MapPlaceTrade(this IEndpointRouteBuilder routes) {
        routes.MapPost("/api/trades", async (
            PlaceTradeRequest request,
            Feature feature,
            CancellationToken token) => {
                var response = await feature.Execute(request,token);
                return Results.Ok(response);
            })
        .WithName("PlaceTrade")
        .WithOpenApi(); // Swagger-friendly

        return routes;
    }
}
