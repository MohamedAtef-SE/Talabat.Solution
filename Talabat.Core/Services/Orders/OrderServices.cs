using AutoMapper;
using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Application.Abstractions.DTOModels.Orders;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Specifications.DeliveryMethods;
using Talabat.Core.Application.Specifications.Orders;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Basket;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Application.Services.Orders
{
    public class OrderServices : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public OrderServices(IUnitOfWork unitOfWork,IPaymentService paymentService, IBasketRepository basketRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<OrderDTO?> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            var basket = await _basketRepository.GetBasketAsync(createOrderDTO.BasketId);

            var OrderItems = new List<OrderItem>();

            if (basket?.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.GetRepository<Product>().GetAsync(item.Id);

                    if(product is null) continue;

                    var OrderedProductItem = new OrderedProductItem(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(OrderedProductItem, product.Price, item.Quantity);

                    OrderItems.Add(orderItem);
                }
            }


            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var shippingAddress = new Address(createOrderDTO.ShippingAddress.FirstName, createOrderDTO.ShippingAddress.LastName, createOrderDTO.ShippingAddress.Street, createOrderDTO.ShippingAddress.City, createOrderDTO.ShippingAddress.Country);

            var specs = new OrderSpecsForPaymentIntent(basket.PaymentIntentId!);

            var exOrder = await _unitOfWork.GetRepository<Order>().GetWithSpecAsync(specs);
            if (exOrder != null)
            {
                var isDeleted = _unitOfWork.GetRepository<Order>().Delete(exOrder);
                if(!isDeleted)
                    throw new Exception("Can't delete ex-order.");
                // Ensure update current order with latest total price.
                var existBasket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basket.Id);
                if (existBasket is null)
                    throw new Exception("Can't update Total Basket amount.");
            }
               

            var newOrder = new Order(createOrderDTO.BuyerEmail, shippingAddress, createOrderDTO.DeliveryMethodId, OrderItems, subTotal,basket.PaymentIntentId!);

            var addedLocally = await _unitOfWork.GetRepository<Order>().AddAsync(newOrder);

            if (addedLocally)
            {
                var savedChangesSuccessfully = await _unitOfWork.CompleteAsync();
                var spec = new OrdersForSpecificUserSpecification(createOrderDTO.BuyerEmail);

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

        public async Task<DeliveryMethodDTO?> GetDeliveryMethodAsync(string? deliveryMethodId)
        {
            var specs = new DeliveryMethodsSpecifications(deliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod>().GetWithSpecAsync(specs);

            if (deliveryMethod is null) return null;

            var mappedDeliveryMethod = _mapper.Map<DeliveryMethodDTO>(deliveryMethod);
            return mappedDeliveryMethod;
        }

    }
}
