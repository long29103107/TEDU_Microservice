using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
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

    app.MigrateDatabase<ProductContext>((context, _) =>
        {
            ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
        })
        .Run();
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