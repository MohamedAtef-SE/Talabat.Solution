using AutoMapper;
using System.Text.Json;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Domain.Contracts;
using Talabat.Shared.DTOModels.Basket;
using Talabat.Shared.DTOModels.Orders;
using Talabat.Shared.Exceptions;

namespace Talabat.Core.Application.Services.Basket
{
    internal class BasketServices(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<CustomerBasketDTO> GetCustomerBasketAsync(string basketId)
        {
            var customerBasket = await _basketRepository.GetBasketAsync(basketId);

            if (customerBasket == null)
                throw new NotFoundException($"no basket found with this id {basketId}");

            var mappedCustomerBasketDTO = _mapper.Map<CustomerBasketDTO>(customerBasket);
            return mappedCustomerBasketDTO;
        }
        public async Task<CustomerBasketDTO> UpdateBasketAsync(CustomerBasketDTO customerBasketDTO)
        {
            var serializedOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var SerializedCustomerBasket = JsonSerializer.Serialize(customerBasketDTO, serializedOptions);

            var customerBasket = await _basketRepository.UpdateBasketAsync(customerBasketDTO.Id, SerializedCustomerBasket);
            if (customerBasket == null)
                throw new BadRequestException($"failed to update customer bakset");

            var mappedBasket = _mapper.Map<CustomerBasketDTO>(customerBasket);

            return mappedBasket;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }
    }
}
