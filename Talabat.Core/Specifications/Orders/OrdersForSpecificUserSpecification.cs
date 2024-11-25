using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Specifications;

namespace Talabat.Core.Application.Specifications.Orders
{
    public class OrdersForSpecificUserSpecification : BaseSpecifications<Order>
    {
        public OrdersForSpecificUserSpecification(string BuyerEmail) : base(O => O.BuyerEmail == BuyerEmail)
        {

        }
        public OrdersForSpecificUserSpecification(string BuyerEmail,string OrderId) : base(O => O.BuyerEmail == BuyerEmail && O.Id.Equals( OrderId))
        {

        }

        protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
