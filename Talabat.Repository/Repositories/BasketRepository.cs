using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Contracts;
using Talabat.Core.Entities.Basket;

namespace Talabat.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public IDatabase _database { get; set; }
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var BasketAsJson = await _database.StringGetAsync(basketId);

            var Basket = BasketAsJson.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(BasketAsJson!);

            return Basket;

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var BasketToJson = JsonSerializer.Serialize(customerBasket);
           var IsCreatedOrUpdated = await _database.StringSetAsync(customerBasket.Id,BasketToJson,TimeSpan.FromDays(1));

            if(!IsCreatedOrUpdated)  return null;

            // return customerBasket; not Recommended because if state of basket got any change in Redis Db, i should return it from Redis Db.
            return await GetBasketAsync(customerBasket.Id);
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var isDeleted = await _database.KeyDeleteAsync(basketId);

            return isDeleted;
        }

    }
}
