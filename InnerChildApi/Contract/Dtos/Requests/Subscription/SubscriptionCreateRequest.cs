using Contract.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Subscription
{
    public class SubscriptionCreateRequest
    {
        [Required(ErrorMessage = "Subscription Type is required")]
        public SubscriptionEnum SubscriptionType { get; set; }
        [Required(ErrorMessage = "Subscription Description is required")]
        public string SubscriptionDescription { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be a positive integer greater than 0")]
        public int SubscriptionPrice { get; set; }
    }
}
