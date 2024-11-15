﻿using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Entities.Basket;

namespace Talabat.Core.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasketDTO customerBasket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
