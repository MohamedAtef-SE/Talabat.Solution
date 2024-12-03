using Talabat.Core.Domain.Entities.Orders;

namespace Talabat.Core.Application.Specifications.Payments
{
    internal class ChangeOrderStatusSpecs : BaseSpecifications<Order,string>
    {
        private string paymentIntentId;

        public ChangeOrderStatusSpecs(string paymentIntentId):base(order=> order.PaymentIntentId.Equals(paymentIntentId))
        {
            this.paymentIntentId = paymentIntentId;
        }
    }
}