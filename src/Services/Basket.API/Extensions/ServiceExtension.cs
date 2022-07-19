using System.Runtime.Serialization;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;

namespace Basket.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddTransient<ISerializeService, SerializeService>();
        return services;
    }
    
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetSection("CacheSettings:ConnectionStrings").Value;
        if(string.IsNullOrEmpty(redisConnectionString))
        {
            throw new ArgumentNullException("Redis Connection string is not configured.");
        }
        
        //Redis Configuration
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });
    }
}