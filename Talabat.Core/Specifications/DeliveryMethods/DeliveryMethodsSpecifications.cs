using Talabat.Core.Domain.Entities.Orders;

namespace Talabat.Core.Application.Specifications.DeliveryMethods
{
    internal class DeliveryMethodsSpecifications : BaseSpecifications<DeliveryMethod,string>
    {
        public DeliveryMethodsSpecifications(string? deliveryMethodId) : base(DM => DM.Id.Equals(deliveryMethodId))
        {

        }
    }
}
