using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Domain.Entities.Basket;

namespace Talabat.Core.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasketDTO customerBasket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
