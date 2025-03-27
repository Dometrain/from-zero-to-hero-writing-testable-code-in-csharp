namespace Exercises
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }

        public decimal Total { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}