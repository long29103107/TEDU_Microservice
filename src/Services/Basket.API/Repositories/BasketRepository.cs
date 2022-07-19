using System.Reflection.Emit;
using System.Text.Json;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCacheService;
    private readonly ISerializeService _serializeService;
    private readonly ILogger _logger;

    public BasketRepository(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger)
    {
        _redisCacheService = redisCacheService;
        _serializeService = serializeService;
        _logger = logger;
    }

    public async Task<Cart?> GetBasketByUsername(string username)
    {
        var basket = await _redisCacheService.GetStringAsync(username);

        return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
    }

    public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
    {
        if (options != null)
        {
            await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart), options);
        }
        else
        {
            await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
        }

        return await GetBasketByUsername(cart.Username);
    }

    public async Task<bool> DeleteBasketFromUsername(string username)
    {
        try
        {
            await _redisCacheService.RemoveAsync(username);
            
            return true;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message);
            throw;
        }
        
    }
}