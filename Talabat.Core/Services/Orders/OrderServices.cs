using AutoMapper;
using Talabat.Core.Application.Abstractions.DTOModels.Orders;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Specifications.Orders;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Application.Services.Orders
{
    public class OrderServices : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public OrderServices(IUnitOfWork unitOfWork, IBasketRepository basketRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<OrderDTO?> CreateOrderAsync(OrderParams orderParams)
        {
            var basket = await _basketRepository.GetBasketAsync(orderParams.BasketId);

            var OrderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product>().GetAsync(item.Id);

                var OrderedProductItem = new OrderedProductItem(product.Id, product.Name, product.PictureUrl);

                var orderItem = new OrderItem(OrderedProductItem, product.Price, item.Quantity);

                OrderItems.Add(orderItem);
            }


            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var shippingAddress = new Address(orderParams.ShippingAddress.FirstName, orderParams.ShippingAddress.LastName, orderParams.ShippingAddress.Street, orderParams.ShippingAddress.City, orderParams.ShippingAddress.Country);

            var newOrder = new Order(orderParams.BuyerEmail, shippingAddress, orderParams.DeliveryMethodId, OrderItems, subTotal);

            var addedLocally = await _unitOfWork.GetRepository<Order>().AddAsync(newOrder);

            if (addedLocally)
            {
                var savedChangesSuccessfully = await _unitOfWork.CompleteAsync();
                var spec = new OrdersForSpecificUserSpecification(orderParams.BuyerEmail);

                if (savedChangesSuccessfully)
                {
                    var currentOrder = await _unitOfWork.GetRepository<Order>().GetWithSpecAsync(spec);
                    var mappedOrder = _mapper.Map<OrderDTO>(currentOrder);

                    return mappedOrder;
                }
            }
            return null;
        }

        public async Task<IReadOnlyList<OrderDTO>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrdersForSpecificUserSpecification(BuyerEmail);

            var orders = await _unitOfWork.GetRepository<Order>().GetAllWithSpecAsync(spec);

            var mappedOrders = _mapper.Map<IReadOnlyList<OrderDTO>>(orders);

            return mappedOrders;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(string BuyerEmail, string OrderId)
        {
            var spec = new OrdersForSpecificUserSpecification(BuyerEmail, OrderId);

            var order = await _unitOfWork.GetRepository<Order>().GetWithSpecAsync(spec);

            var mappedOrder = _mapper.Map<OrderDTO>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethodDTO>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod>().GetAllAsync();

            var mappedDeliveryMethods = _mapper.Map<IReadOnlyList<DeliveryMethodDTO>>(deliveryMethods);
            return mappedDeliveryMethods;
        }

    }
}
