using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Application.Abstractions.DTOModels.Orders;

namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IPaymentService
    {
        // Create Or Update PaymentIntentId
        Task<CustomerBasketDTO?> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<OrderDTO> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool isPaid);
    }
}
