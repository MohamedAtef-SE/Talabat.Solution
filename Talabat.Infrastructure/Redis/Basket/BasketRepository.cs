using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Basket;

namespace Talabat.Infrastructure.Redis.Basket
{
    public class BasketRepository : IBasketRepository
    {
        public IDatabase _database { get; set; }
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
           
        }
        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var SerializedCustomerBasket = await _database.StringGetAsync(basketId);
            

            if (SerializedCustomerBasket.IsNull)
            {
                var customerBasket = new CustomerBasket(basketId);

                var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var customerBasketSerialized = JsonSerializer.Serialize(customerBasket, jsonSerializerOptions);

                return await UpdateBasketAsync(basketId, customerBasketSerialized);
            }
            else
            {
                var basket = JsonSerializer.Deserialize<CustomerBasket>(SerializedCustomerBasket!);
     
                return basket is not null ? basket : new CustomerBasket(basketId);
            }


        }
        public async Task<CustomerBasket> UpdateBasketAsync(string basketId, string SerializedCustomerBasket)
        {
            await _database.StringSetAsync(basketId, SerializedCustomerBasket, TimeSpan.FromDays(1));

            return await GetBasketAsync(basketId);
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

    }
}
