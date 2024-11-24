using Talabat.Core.Application.Abstractions.DTOModels.Orders;

namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<OrderDTO?> CreateOrderAsync(OrderParams orderParams);
        Task<IReadOnlyList<OrderDTO>> GetOrdersForSpecificUserAsync(string BuyerEmail);
        Task<OrderDTO> GetOrderByIdAsync(string BuyerEmail,string OrderId);
        Task<IReadOnlyList<DeliveryMethodDTO>> GetAllDeliveryMethodsAsync();

    }
}
