namespace Exercises
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public int Status { get; set; } // 0 = Pending, 1 = Processing, 2 = Shipped, 3 = Completed, 4 = Cancelled
    }
}