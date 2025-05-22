namespace Contract.Dtos.Requests.Payment
{
    public class PaymentRequest
    {
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public long OrderCode { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public List<PaymentItem> PaymentItems { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public long? ExpiredAt { get; set; } 

    }
    public class PaymentItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
