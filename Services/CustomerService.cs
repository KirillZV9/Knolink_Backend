namespace PomogatorAPI.Repositories

{
    using Google.Cloud.Firestore;
    using PomogatorAPI.Interfaces;
    using PomogatorAPI.Models;
    using static Config;
    

    public class CustomerService : ICustomer
    {
        public List<Customer> Customers {get; set;} = new List<Customer>();

        private readonly FirestoreDb db =  FirestoreDb.Create(fbProjectId);

        private const string fbCollection = "сustomers";


        async public Task DeleteAsync(string id)
        {
            if (DoesCustomerExistAsync(id).Result)
            {
                DocumentReference customerRef = db.Collection(fbCollection).Document(id);
                DocumentSnapshot snapshot = await customerRef.GetSnapshotAsync();
                Customer customer = snapshot.ConvertTo<Customer>();
                this.Customers.Add(customer);
                await customerRef.DeleteAsync();
            }
            else
                throw new ArgumentException();
        }

        public async Task<bool> DoesCustomerExistAsync(string id)
        {
            DocumentReference docRef = db.Collection(fbCollection).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
                return true;
            return false;
        }


        public async Task PostAsync(string id, string telNum, string name)
        {
            Customer customer = new Customer(id, telNum, name);

            if (!DoesCustomerExistAsync(customer.Id).Result)
            {
                Customers.Add(customer);

                DocumentReference docRef = db.Collection(fbCollection).Document(customer.Id);
                Dictionary<string, object> _customer = new Dictionary<string, object>(){
                    {"Id", customer.Id},
                    {"Name", customer.Name},
                    {"TelNum", customer.TelNum },
                    {"Balance", customer.Balance }
                };

                await docRef.SetAsync(_customer);
            }
            else
                throw new ArgumentException();
        }


        public async Task UpdateAsync(Customer customerUpdated)
        {
            if (DoesCustomerExistAsync(customerUpdated.Id).Result)
            {
                DocumentReference customerRef = db.Collection(fbCollection).Document(customerUpdated.Id);
                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    {"Id", customerUpdated.Id},
                    {"Name", customerUpdated.Name},
                    {"TelNum", customerUpdated.TelNum},
                    {"Balance", customerUpdated.Balance}

                };

                await customerRef.UpdateAsync(updates);
                Customers.Add(customerUpdated);
            }
            else
                throw new ArgumentException();
        }

        public async Task GetAsync(string id)
        {
            if (DoesCustomerExistAsync(id).Result)
            {
                DocumentReference docRef = db.Collection(fbCollection).Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                Customer customer = snapshot.ConvertTo<Customer>();
                Customers.Add(customer);
            }
            else
                throw new ArgumentException();
        }
    }
}
