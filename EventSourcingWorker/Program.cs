using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using Serilog.Sinks.Syslog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Syslog("logs6.papertrailapp.com", 12345, 
        facility: Facility.Local0, 
        format: SyslogFormat.RFC5424)
    .CreateLogger();

try
{
    Log.Information("Starting worker host...");

    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices(services =>
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient("mongodb://mongo:27017"));
            services.AddHostedService<Worker>();
        })
        .Build()
        .Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Worker failed");
}
finally
{
    Log.CloseAndFlush();
}