using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(b => b
        .AddService(serviceName: "Callee", serviceVersion: "1.0.0"))
    .WithTracing(b => b
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(o => o.Protocol = OtlpExportProtocol.HttpProtobuf));
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestPath |
                      HttpLoggingFields.RequestMethod |
                      HttpLoggingFields.RequestBody |
                      HttpLoggingFields.ResponseBody |
                      HttpLoggingFields.ResponseStatusCode;
});
builder.Host.UseSerilog((_, lc) => lc
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Warning)
    .Enrich.WithTraceIdAndSpanId()
    .WriteTo.OpenTelemetry(endpoint: "http://localhost:4318/v1/logs",
        protocol: OpenTelemetrySink.OtlpProtocol.HttpProtobuf,
        resourceAttributes: new Dictionary<string, object>
        {
            { "service.name", "Callee" }
        })
);

var app = builder.Build();
app.UseHttpLogging();
app.MapControllers();
app.Run();
