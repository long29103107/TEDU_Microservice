using Common.Logging;
using Product.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Information("Start Product Api up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.UseInfrastructure();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhanded exception");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}