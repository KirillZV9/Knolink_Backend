namespace PomogatorAPI.Models
{
    public class Order
    {
        public string CustomerId { get; set; } = string.Empty;
        public string? TutorId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double? Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime? Closed { get; set; } = DateTime.MinValue;

        public Order(string customerId, string subject, string description)
        {
            CustomerId = customerId;
            TutorId = null;
            Subject = subject;
            Description = description;
            Price = null;
            Status = "open";
            Created = DateTime.Now;
            Closed = null;
        }

        public Order() { }

    }
}
