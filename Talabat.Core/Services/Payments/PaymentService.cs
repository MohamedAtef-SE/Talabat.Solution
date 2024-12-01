using AutoMapper;
using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Application.Abstractions.DTOModels.Orders;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Specifications.Orders;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Orders;

namespace Talabat.Core.Application.Services.Payments
{
    public class PaymentService(IBasketRepository _basketRepository, IMapper _mapper, IUnitOfWork _unitOfWork, IConfiguration _configuration) : IPaymentService
    {
        public async Task<CustomerBasketDTO?> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Secret Key Configuration
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["Secretkey"];

            // Get Basket
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null)
                throw new Exception($"No basket found with this {basketId} id");

            var shippingPrice = 0.0M;

            if (basket.DeliveryMethodId is not null)
            {
                var deliveryMethodDTO = await _unitOfWork.GetRepository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId);

                if (deliveryMethodDTO is not null)
                    shippingPrice = deliveryMethodDTO.Cost;
            }

            // Can't depend directly on Client-side prices. i need to retrive data from db.
            //var subtotal = basket.Items.Sum(item => item.Quantity * item.Price);

            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.GetRepository<Domain.Entities.Products.Product>().GetAsync(item.Id);

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

            var customerBasket = await _basketRepository.UpdateBasketAsync(basketDTO);

            basketDTO = _mapper.Map<CustomerBasketDTO>(customerBasket);

            return basketDTO is not null ? basketDTO : null;
        }

        public async Task<OrderDTO> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool isPaid)
        {
            if (isPaid)
            {

                return await ChangeOrderStatusAsync(PaymentIntentId,OrderStatus.PaymentReceived);
            }
            else
            {
                return await ChangeOrderStatusAsync(PaymentIntentId, OrderStatus.PaymentFailed);
            }
        }

        private async Task<OrderDTO> ChangeOrderStatusAsync(string paymentIntentId,OrderStatus orderStatus)
        {
            var specs = new OrderSpecsForPaymentIntent(paymentIntentId);
            var order = await _unitOfWork.GetRepository<Order>().GetWithSpecAsync(specs);
            if (order is null) throw new Exception("Failed to Get Order By PatmentIntentId.");

            order.Status = orderStatus;
            _unitOfWork.GetRepository<Order>().Update(order);

            var updated = await _unitOfWork.CompleteAsync();

            if (!updated) throw new Exception("Failed to Update the Order Status");

            var mappedOrder = _mapper.Map<OrderDTO>(order);
            return mappedOrder;
        }
    }
}
