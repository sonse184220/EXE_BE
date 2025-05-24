using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Payment
{
    public class BuySubscriptionRequest
    {
        [Required(ErrorMessage = "subscription id is required!")]
        public string SubscriptionId { get; set; }
    }
}
