using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Logging;
using Serilog;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} Api up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.Configure <RouteOptions > (options => 
            options.LowercaseUrls = true);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    //var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    //builder.Services.AddDbContext<CustomerContext>(options => options.UseNpgsql(connectionString));
    // var config = new MapperConfiguration(cfg =>
    // {
    //     cfg.AddProfile(new MappingProfile());
    // });
    //
    // var mapper = config.CreateMapper();
    // builder.Services.AddSingleton(mapper);
    // builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
    //builder.Services.AddScoped(typeof(IRepositoryQueryBase<,,>), typeof(RepositoryQueryBase<,,>));
    
    builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
    {
        var dataAccess = Assembly.GetExecutingAssembly();
        builder.RegisterAssemblyTypes(dataAccess)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(dataAccess)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    });
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
            $"{builder.Environment.ApplicationName} v1"));
    }
    
    //app.UseHttpsRedirection();

    app.UseAuthorization();
    app.MapDefaultControllerRoute();    
    
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if(type.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;
    Log.Fatal(ex, "Unhanded exception");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} API complete");
    Log.CloseAndFlush();
}