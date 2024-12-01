using Talabat.Core.Application.Abstractions.DTOModels.Orders;

namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<OrderDTO?> CreateOrderAsync(CreateOrderDTO orderParams);
        Task<IReadOnlyList<OrderDTO>> GetOrdersForSpecificUserAsync(string BuyerEmail);
        Task<OrderDTO> GetOrderByIdAsync(string BuyerEmail,string OrderId);
        Task<IReadOnlyList<DeliveryMethodDTO>> GetAllDeliveryMethodsAsync();
        Task<DeliveryMethodDTO?> GetDeliveryMethodAsync(string? deliveryMethodId);

    }
}
