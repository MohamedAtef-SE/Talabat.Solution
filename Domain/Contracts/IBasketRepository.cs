using Talabat.Core.Domain.Entities.Basket;

namespace Talabat.Core.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        Task<CustomerBasket> UpdateBasketAsync(string basketId, string SerializedCustomerBasket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
