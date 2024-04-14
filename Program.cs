using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context,configuration) =>
{
    configuration.Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(
            new ElasticsearchSinkOptions(
                    new Uri(context.Configuration["ElasticConfig:Uri"])
                )
            {
                IndexFormat = $"{context.Configuration["AppName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace( ",", "-")}-{DateTime.UtcNow: yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            }
        ).Enrich.WithProperty("Enviroment",context.HostingEnvironment.EnvironmentName)
        .ReadFrom.Configuration(context.Configuration); 

});
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenTelemetry()
.WithMetrics(x =>
    x
        .AddPrometheusExporter(options => options.DisableTotalNameSuffixForCounters = true)
        .AddRuntimeInstrumentation()
        .AddAspNetCoreInstrumentation()
    
);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
