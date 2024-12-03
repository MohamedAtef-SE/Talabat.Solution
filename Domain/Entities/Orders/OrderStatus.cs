using System.Runtime.Serialization;

namespace Talabat.Core.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 1,

        [EnumMember(Value = "Payment Received")]
        PaymentReceived = 2,

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed = 3,
    }
}
