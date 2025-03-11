using PostExecutionMonitoringService.Models;
using PostExecutionMonitoringService.Services;

var builder = WebApplication.CreateBuilder(args);


// Register the execution monitoring service with DI.
builder.Services.AddSingleton<IExecutionMonitoringService, ExecutionMonitoringService>();

var app = builder.Build();

// Endpoint to record a new execution log.
// Example POST /logs with JSON body representing an ExecutionLog.
app.MapPost("/logs", async (ExecutionLog log, IExecutionMonitoringService monitoringService) => {
    await monitoringService.RecordExecutionLogAsync(log);
    return Results.Ok(new { Message = "Execution log recorded successfully." });
});

// Endpoint to retrieve all execution logs.
// Example GET /logs
app.MapGet("/logs", async (IExecutionMonitoringService monitoringService) => {
    var logs = await monitoringService.GetExecutionLogsAsync();
    return Results.Ok(logs);
});

app.Run();
