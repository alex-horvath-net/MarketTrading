namespace TradingService.PlaceTrade;

public static class Endpoint {
    public static IEndpointRouteBuilder MapPlaceTrade(this IEndpointRouteBuilder routes) {
        routes.MapPost("/api/trades", async (
            PlaceTradeRequest request,
            Feature handler,
            CancellationToken ct) => {
                var response = await handler.Execute(request, ct);
                return Results.Ok(response);
            })
        .WithName("PlaceTrade")
        .WithOpenApi(); // Swagger-friendly

        return routes;
    }
}
