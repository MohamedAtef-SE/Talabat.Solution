using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Specifications;

namespace Talabat.Core.Application.Specifications.Orders
{
    public class OrdersForSpecificUserSpecification : BaseSpecifications<Order>
    {
        public OrdersForSpecificUserSpecification(string BuyerEmail) : base(O => O.BuyerEmail.Equals(BuyerEmail))
        {

        }
        public OrdersForSpecificUserSpecification(string BuyerEmail, string OrderId) : base(O => O.BuyerEmail.Equals(BuyerEmail) && O.Id.Equals(OrderId))
        {

        }

        #region Using Factory Methods to Handle Different Constructors accept same parameters, But in this case will not work becasue we should change Func too
        //public static OrdersForSpecificUserSpecification CreateWithBuyerEmailParam(string BuyerEmail)
        //{
        //    return new OrdersForSpecificUserSpecification(BuyerEmail);
        //}

        //public static OrdersForSpecificUserSpecification CreateWithPaymentInentParam(string PaymentInent)
        //{
        //    return new OrdersForSpecificUserSpecification(PaymentInent);
        //}
        #endregion

        protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
