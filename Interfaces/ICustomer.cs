using PomogatorAPI.Models;

namespace PomogatorAPI.Interfaces
{
    public interface ICustomer
    {
        List<Customer> Customers {get; set;}
        Task GetAsync(string id);
        Task PostAsync(string id, string telNum, string name);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(string id);

        Task PostASS();
    }
}
