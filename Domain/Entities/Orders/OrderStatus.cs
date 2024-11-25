using System.Runtime.Serialization;

namespace Talabat.Core.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 1,

        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived = 2,

        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed = 3,
    }
}
