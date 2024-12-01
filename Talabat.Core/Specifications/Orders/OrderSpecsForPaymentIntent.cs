using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Specifications;

namespace Talabat.Core.Application.Specifications.Orders
{
    internal class OrderSpecsForPaymentIntent : BaseSpecifications<Order>
    {
        public OrderSpecsForPaymentIntent(string paymentIntentId):base(order => order.PaymentIntentId.Equals(paymentIntentId))
        {

        }
    }
}
