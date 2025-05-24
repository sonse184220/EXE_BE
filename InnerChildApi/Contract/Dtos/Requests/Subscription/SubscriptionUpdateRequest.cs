using Contract.Common.Enums;

namespace Contract.Dtos.Requests.Subscription
{
    public class SubscriptionUpdateRequest
    {
        public SubscriptionEnum? SubscriptionType { get; set; }
        public string? SubscriptionDescription { get; set; }
        public double? SubscriptionPrice { get; set; }
    }
}
