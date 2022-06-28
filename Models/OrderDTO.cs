namespace WebApi.Models
{
    public class OrderDTO
    {
        public string CustomerId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

    }
}
