namespace PomogatorAPI.Repositories

{
    using Google.Cloud.Firestore;
    using PomogatorAPI.Interfaces;
    using PomogatorAPI.Models;
    using static Config;
    

    public class CustomerRepository : ICustomer
    {
        List<Customer> _customerList = new List<Customer> { };

        FirestoreDb db =  FirestoreDb.Create(fbProjectId);

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesCustomerExist(int id)
        {
            throw new NotImplementedException();
        }


        public async Task PostAsync(Customer customer)
        {
            DocumentReference docRef = db.Collection("Customers").Document(customer.Id);
            Dictionary<string, object> _customer = new Dictionary<string, object>()
            {
                {"Id", customer.Id},
                {"Name", customer.Name},
                {"TelNum", customer.TelNum },
                {"Balance", customer.Balance },
                {"Status", customer.Status }
            };
            await docRef.SetAsync(_customer);
        }

        public async Task PostASS()
        {
            DocumentReference docRef = db.Collection("Customers").Document("TEST");
            Dictionary<string, object> _customer = new Dictionary<string, object>()
            {
                {"Id", "1488"},
                {"Name", "BEN"},
                {"TelNum", "NO"},
                {"Balance", 0},
                {"Status", "YES"}
            };
            await docRef.SetAsync(_customer);
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
