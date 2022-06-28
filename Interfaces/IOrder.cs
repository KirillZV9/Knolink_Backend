using PomogatorAPI.Models;

namespace PomogatorAPI.Interfaces
{
    public interface IOrder
    {
        Order Order { get; set; }
        List<Order> OrdersList { get; set; }
        Dictionary<string, string> RespondedTutors { get; set; }
        Task PostAsync(string customerId, string subject, string description, string type);
        Task GetAsync(string id);
        Task GetOrdersById(string customerId, string atribute);
        Task GetOrdersBySubject(string subject);
        Task SetTutor(string orderId, string tutorId);
        Task CloseOrder(string orderId);
        Task PostResponse(string orderId, string tutorId, string price);
        Task<Dictionary<string, string>> GetAllRespondedTutors(string orderId);
        Task PostTutorRating(string orderId, int rating);
        Task CountTutorRating(string tutorId);
        Task UpdateRatingAsync(string id, double rating);
    }
}
