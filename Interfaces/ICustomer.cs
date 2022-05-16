using PomogatorAPI.Models;

namespace PomogatorAPI.Interfaces
{
    public interface ICustomer
    {
        Customer Get(int id);
        bool DoesCustomerExist(int id);
        Task PostAsync(Customer customer);
        void Update(Customer customer);
        void Delete(int id);

        Task PostASS();
    }
}
