using AutoMapper;
using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Contracts;
using Talabat.Core.Entities.Basket;

namespace Talabat.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IMapper _mapper;

        public IDatabase _database { get; set; }
        public BasketRepository(IConnectionMultiplexer redis,IMapper mapper)
        {
            _database = redis.GetDatabase();
            _mapper = mapper;
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var BasketAsJson = await _database.StringGetAsync(basketId);

            var Basket = BasketAsJson.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(BasketAsJson!);

            return Basket;

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasketDTO customerBasket)
        {
            var mappedCustomerBasket = _mapper.Map<CustomerBasket>(customerBasket);
            var BasketToJson = JsonSerializer.Serialize(mappedCustomerBasket);
           var IsCreatedOrUpdated = await _database.StringSetAsync(mappedCustomerBasket.Id,BasketToJson,TimeSpan.FromDays(1));

            if(!IsCreatedOrUpdated)  return null;

            // return customerBasket; not Recommended because if state of basket got any change in Redis Db, i should return it from Redis Db.
            return await GetBasketAsync(mappedCustomerBasket.Id);
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var isDeleted = await _database.KeyDeleteAsync(basketId);

            return isDeleted;
        }

    }
}
