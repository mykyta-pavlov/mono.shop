using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "В очікуванні")]
        Pending,

        [EnumMember(Value = "Платіж отримано")]
        PaymentReceived,

        [EnumMember(Value = "Платіж скасовано")]
        PaymentFailed
    }
}
