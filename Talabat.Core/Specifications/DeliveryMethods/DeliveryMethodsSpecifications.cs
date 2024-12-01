using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Specifications;

namespace Talabat.Core.Application.Specifications.DeliveryMethods
{
    internal class DeliveryMethodsSpecifications : BaseSpecifications<DeliveryMethod>
    {
        public DeliveryMethodsSpecifications(string? deliveryMethodId) : base(DM => DM.Id.Equals(deliveryMethodId))
        {

        }
    }
}
