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
        _logger.Information($"BEGIN: GetBasketByUsername {username}");
        var basket = await _redisCacheService.GetStringAsync(username);
        _logger.Information($"END: GetBasketByUsername {username}");

        return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
    }

    public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
    {
        _logger.Information($"BEGIN: UpdateBasket for {cart.Username}");
        if (options != null)
        {
            await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart), options);
        }
        else
        {
            await _redisCacheService.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
        }
        _logger.Information($"END: UpdateBasket for {cart.Username}");

        return await GetBasketByUsername(cart.Username);
    }

    public async Task<bool> DeleteBasketFromUsername(string username)
    {
        try
        {
            _logger.Information($"BEGIN: DeleteBasketFromUsername {username}");
            await _redisCacheService.RemoveAsync(username);
            _logger.Information($"END: DeleteBasketFromUsername {username}");
            
            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Error DeleteBasketFromUsername {e.Message}");
            throw;
        }
        
    }
}