using Google.Cloud.Firestore;

namespace PomogatorAPI.Models
{
    [FirestoreData]
    public class Order
    {
        [FirestoreProperty]
        public string CustomerId { get; set; } = string.Empty;
        [FirestoreProperty]
        public string? TutorId { get; set; }
        [FirestoreProperty]
        public string Subject { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Description { get; set; } = string.Empty;
        [FirestoreProperty]
        public string? Price { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Status { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Type { get; set; } = string.Empty;
        [FirestoreProperty]
        public DateTime Created { get; set; } = DateTime.MinValue;
        [FirestoreProperty]
        public DateTime? Closed { get; set; } = DateTime.MinValue;
        public string Id { get; set; } = string.Empty;

        public Order(string customerId, string subject, string description, string type)
        {
            CustomerId = customerId;
            TutorId = null;
            Subject = subject;
            Description = description;
            Price = null;
            Status = "open";
            Created = DateTime.UtcNow;
            Closed = null;
            Type = type;
        }

        public Order() { }

    }
}
