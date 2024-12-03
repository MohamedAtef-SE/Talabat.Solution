using Talabat.Shared.DTOModels.Basket;
using Talabat.Shared.DTOModels.Orders;


namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        // Create Or Update PaymentIntentId
        Task<CustomerBasketDTO?> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<OrderDTO> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool isPaid);
    }
}
