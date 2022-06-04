

namespace PomogatorAPI.Repositories
{
    using PomogatorAPI.Models;
    using PomogatorAPI.Interfaces;
    using static Config;
    using Google.Cloud.Firestore;

    public class OrderService
    {
        public Order Order { get; set; } = new Order();
        public Dictionary<string, Order> OrdersDict { get; set; } = new Dictionary<string, Order>();

        private readonly FirestoreDb db = FirestoreDb.Create(fbProjectId);

        private const string fbCollection = "orders";


        public async Task PostAsync(string customerId, string subject, string description)
        {
            Order order = new Order(customerId, subject, description);

            DocumentReference docRef = db.Collection(fbCollection).Document();

            Dictionary<string, object> _order = new Dictionary<string, object>(){
                    {"CustomerId", order.CustomerId},
                    {"TutorId", order.TutorId},
                    {"Subject", order.Subject},
                    {"Description", order.Description},
                    {"Price", order.Price},
                    {"Status", order.Status},
                    {"Created", order.Created},
                    {"Closed", order.Closed}
                };

            await docRef.SetAsync(_order);
        }

        public async Task GetAsync(string id)
        {
            DocumentReference docRef = db.Collection(fbCollection).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Order = snapshot.ConvertTo<Order>();
            }
            else
                throw new Exception();
        }

        public async Task GetOrdersByCustomerId(string cutomerId)
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo("CustomerId", cutomerId);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();
            if(ordersQuerySnapshot.Count > 0)
            {
                foreach(DocumentSnapshot snapshot in ordersQuerySnapshot.Documents)
                {
                    Order order = snapshot.ConvertTo<Order>();
                    OrdersDict.Add(snapshot.Id, order);
                }
            }
            else
                throw new Exception();
        }

        public async Task GetOrdersByTutorId(string tutorId)
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo("TutorId", tutorId);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();
            if (ordersQuerySnapshot.Count > 0)
            {
                foreach (DocumentSnapshot snapshot in ordersQuerySnapshot.Documents)
                {
                    Order order = snapshot.ConvertTo<Order>();
                    OrdersDict.Add(snapshot.Id, order);
                }
            }
            else
                throw new Exception();
        }

        public async Task GetOrdersBySubject(string subject)
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo("Status", "open").WhereEqualTo("Subject", subject);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();
            if (ordersQuerySnapshot.Count > 0)
            {
                foreach (DocumentSnapshot snapshot in ordersQuerySnapshot.Documents)
                {
                    Order order = snapshot.ConvertTo<Order>();
                    OrdersDict.Add(snapshot.Id, order);
                }
            }
            else
                throw new Exception();
        }

        public async Task SetTutor(string orderId, string tutorId)
        {
            DocumentReference docRef = db.Collection(fbCollection).Document(orderId);

            if (docRef != null)
                await docRef.UpdateAsync("TutorId", tutorId);
            else
                throw new Exception();
        }



    }
}
