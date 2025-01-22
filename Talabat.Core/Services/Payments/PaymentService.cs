using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Specifications.Payments;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Shared.DTOModels.Basket;
using Talabat.Shared.DTOModels.Orders;
using Talabat.Shared.Exceptions;

namespace Talabat.Core.Application.Services.Payments
{
    internal class PaymentService(IServiceManager _serviceManager, ILogger<PaymentService> _logger, IMapper _mapper, IUnitOfWork _unitOfWork, IConfiguration _configuration) : IPaymentService
    {
        public async Task<CustomerBasketDTO?> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Secret Key Configuration
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["Secretkey"];

            try
            {
                // Get Basket
                var basket = await _serviceManager.BasketService.GetCustomerBasketAsync(basketId);

                if (basket is null)
                    throw new NotFoundException($"No basket found with this {basketId} id");

                var shippingPrice = 0.0M;

                if (basket.DeliveryMethodId is not null)
                {
                    var deliveryMethodDTO = await _unitOfWork.GetRepository<DeliveryMethod,string>().GetAsync(basket.DeliveryMethodId);

                    if (deliveryMethodDTO is null)
                        throw new NotFoundException($"No Delivery method found with this specific {basket.DeliveryMethodId} id.");

                    shippingPrice = deliveryMethodDTO.Cost;
                }

                // Can't depend directly on Client-side prices. i need to retrive data from db.
                //var subtotal = basket.Items.Sum(item => item.Quantity * item.Price);

                if (basket.Items.Count() > 0)
                {
                    foreach (var item in basket.Items)
                    {
                        var product = await _unitOfWork.GetRepository<Domain.Entities.Products.Product,string>().GetAsync(item.Id);
                        if (product is null)
                            throw new NotFoundException($"No Product found with this specific {item.Id} id.");

                        if (product is not null && item.Price != product.Price)
                            item.Price = product.Price;
                    }
                }

                var subtotal = basket.Items.Sum(item => item.Quantity * item.Price);
                var TotalOrderCost = subtotal + shippingPrice;

                var Service = new PaymentIntentService();

                if (String.IsNullOrEmpty(basket.PaymentIntentId))
                { // Create PaymentIntent

                    var options = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)TotalOrderCost * 100,
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card" }
                    };

                    PaymentIntent paymentIntent = await Service.CreateAsync(options);
                    basket.PaymentIntentId = paymentIntent.Id;
                    basket.ClientSecret = paymentIntent.ClientSecret;
                }
                else
                { // Update PaymentIntent
                    var options = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)TotalOrderCost * 100,
                    };

                    PaymentIntent paymentIntent = await Service.UpdateAsync(basket.PaymentIntentId, options);

                }

                var basketDTO = _mapper.Map<CustomerBasketDTO>(basket);

                var customerBasket = await _serviceManager.BasketService.UpdateBasketAsync(basketDTO);
                if (customerBasket is null)
                    throw new NotFoundException($"Issue found while getting user basket in payment proccess.");

                basketDTO = _mapper.Map<CustomerBasketDTO>(customerBasket);

                return basketDTO is not null ? basketDTO : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<OrderDTO> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool isPaid)
        {
            if (isPaid)
            {

                return await ChangeOrderStatusAsync(PaymentIntentId, OrderStatus.PaymentReceived);
            }
            else
            {
                return await ChangeOrderStatusAsync(PaymentIntentId, OrderStatus.PaymentFailed);
            }
        }

        private async Task<OrderDTO> ChangeOrderStatusAsync(string paymentIntentId, OrderStatus orderStatus)
        {
            try
            {
                var specs = new ChangeOrderStatusSpecs(paymentIntentId);
                var order = await _unitOfWork.GetRepository<Order,string>().GetWithSpecAsync(specs);
                if (order is null)
                    throw new Exception("Failed to Get Order By PatmentIntentId.");

                order.Status = orderStatus;
                _unitOfWork.GetRepository<Order,string>().Update(order);

                var updated = await _unitOfWork.CompleteAsync();

                if (!updated)
                    throw new Exception("Failed to Update the Order Status");

                var mappedOrder = _mapper.Map<OrderDTO>(order);
                return mappedOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

    }
}
