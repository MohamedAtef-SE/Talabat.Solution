using AutoMapper;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Specifications.DeliveryMethods;
using Talabat.Core.Application.Specifications.Orders;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Shared.DTOModels.Orders;
using Talabat.Shared.Exceptions;

namespace Talabat.Core.Application.Services.Orders
{
    internal class OrderServices : IOrderService
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

            if (basket is null)
                throw new NotFoundException($"while creating order no basket found with this id {createOrderDTO.BasketId}");

            var OrderItems = new List<OrderItem>();

            if (basket?.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.GetRepository<Product,string>().GetAsync(item.Id);

                    if(product is null)
                        throw new NotFoundException($"while creating order no order found with this id {item.Id}");
                    
                    var OrderedProductItem = new OrderedProductItem(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(OrderedProductItem, product.Price, item.Quantity);

                    OrderItems.Add(orderItem);
                }
            }


            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var shippingAddress = new Address(createOrderDTO.ShippingAddress.FirstName, createOrderDTO.ShippingAddress.LastName, createOrderDTO.ShippingAddress.Street, createOrderDTO.ShippingAddress.City, createOrderDTO.ShippingAddress.Country);

            var exOrder = await _unitOfWork.GetRepository<Order,string>().GetAsync(basket!.PaymentIntentId!);
            if (exOrder != null)
            {
                var isDeleted = _unitOfWork.GetRepository<Order,string>().Delete(exOrder);
                if (!isDeleted)
                    throw new BadRequestException($"while creating order can not delete ex-order");
                // Ensure update current order with latest total price.
                var existBasket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basket.Id);

                if (existBasket is null)
                    throw new Exception("while creating order can't update Total Basket amount after ex-order deleted successfully.");
            }


            var newOrder = new Order(createOrderDTO.BuyerEmail, shippingAddress, createOrderDTO.DeliveryMethodId, OrderItems, subTotal, basket.PaymentIntentId!);

            var addedLocally = await _unitOfWork.GetRepository<Order,string>().AddAsync(newOrder);

            var savedChangesSuccessfully = await _unitOfWork.CompleteAsync();

            if (!addedLocally || !savedChangesSuccessfully)
                throw new BadRequestException($"while creating order failed to add it to Database.");

            var spec = new OrdersForSpecificUserSpecification(createOrderDTO.BuyerEmail);

            var currentOrder = await _unitOfWork.GetRepository<Order,string>().GetWithSpecAsync(spec);
            var mappedOrder = _mapper.Map<OrderDTO>(currentOrder);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<OrderDTO>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrdersForSpecificUserSpecification(BuyerEmail);

            var orders = await _unitOfWork.GetRepository<Order,string>().GetAllWithSpecAsync(spec);

            var mappedOrders = _mapper.Map<IReadOnlyList<OrderDTO>>(orders);

            return mappedOrders;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(string BuyerEmail, string OrderId)
        {
            var spec = new OrdersForSpecificUserSpecification(BuyerEmail, OrderId);

            var order = await _unitOfWork.GetRepository<Order,string>().GetWithSpecAsync(spec);

            var mappedOrder = _mapper.Map<OrderDTO>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethodDTO>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod,string>().GetAllAsync();

            var mappedDeliveryMethods = _mapper.Map<IReadOnlyList<DeliveryMethodDTO>>(deliveryMethods);
            return mappedDeliveryMethods;
        }

        public async Task<DeliveryMethodDTO?> GetDeliveryMethodAsync(string? deliveryMethodId)
        {
            var specs = new DeliveryMethodsSpecifications(deliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod,string>().GetWithSpecAsync(specs);

            if (deliveryMethod is null) return null;

            var mappedDeliveryMethod = _mapper.Map<DeliveryMethodDTO>(deliveryMethod);
            return mappedDeliveryMethod;
        }

    }
}
