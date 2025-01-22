
using Talabat.Shared.DTOModels.Basket;

namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IBasketService
    {
        Task<CustomerBasketDTO> GetCustomerBasketAsync(string basketId);
        Task<CustomerBasketDTO> UpdateBasketAsync(CustomerBasketDTO customerBasketDTO);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}