

namespace PomogatorAPI.Repositories
{
    using PomogatorAPI.Models;
    using PomogatorAPI.Interfaces;
    using static Config;
    using Google.Cloud.Firestore;

    public class OrderService : IOrder
    {
        public Order Order { get; set; } = new Order();

        public Dictionary<string, Order> OrdersDict { get; set; } = new Dictionary<string, Order>();

        public List<Tutor> RespondedTutors { get; set; } = new List<Tutor>();

        private readonly FirestoreDb db = FirestoreDb.Create(fbProjectId);

        private const string fbCollection = "orders";


        public async Task PostAsync(string customerId, string subject, string description, string type)
        {
            Order order = new Order(customerId, subject, description, type);

            DocumentReference docRef = db.Collection(fbCollection).Document();

            Dictionary<string, object> _order = new Dictionary<string, object>(){
                    {"CustomerId", order.CustomerId},
                    {"TutorId", order.TutorId},
                    {"Subject", order.Subject},
                    {"Description", order.Description},
                    {"Price", order.Price},
                    {"Status", order.Status},
                    {"Created", order.Created},
                    {"Closed", order.Closed},
                    {"Type", order.Type}
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

        public async Task GetOrdersById(string cutomerId, string atribute)
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo("CustomerId", cutomerId);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();
            SetOrdersDict(ordersQuerySnapshot);
        }

        public async Task GetOrdersBySubject(string subject)
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo("Status", "open").WhereEqualTo("Subject", subject);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();
            SetOrdersDict(ordersQuerySnapshot);
        }

        private void SetOrdersDict(QuerySnapshot ordersQuerySnapshot)
        {
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

        public async Task CloseOrder(string orderId)
        {
            DocumentReference docRef = db.Collection(fbCollection).Document(orderId);

            if (docRef != null)
            {
                await docRef.UpdateAsync("Status", "closed");
                await docRef.UpdateAsync("Closed", DateTime.Now);
            }
            else
                throw new Exception();
        }

        public async Task PostResponse(string orderId, string tutorId)
        {
            Query responseQuery = db.Collection("order_response").WhereEqualTo("OrderId", orderId).WhereEqualTo("TutorId", tutorId);
            QuerySnapshot responseQuerySnapshot = await responseQuery.GetSnapshotAsync();

            if (responseQuerySnapshot.Count != 0)
                throw new Exception();

            DocumentReference docRef = db.Collection("order_response").Document();

            Dictionary<string, object> _orderResponse = new Dictionary<string, object>(){
                    {"OrderId", orderId},
                    {"TutorId", tutorId}
                };

            await docRef.SetAsync(_orderResponse);
        }

        public async Task<List<string>> GetAllRespondedTutors(string orderId)
        {
            Query ordersQuery = db.Collection("order_response").WhereEqualTo("OrderId", orderId);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();

            if (ordersQuerySnapshot.Count > 0)
            {
                List<string> tutorsId = new List<string>();

                foreach (DocumentSnapshot snapshot in ordersQuerySnapshot.Documents)
                {
                    tutorsId.Add(snapshot.GetValue<string>("TutorId"));
                }

                return tutorsId;
            }
            else
                throw new Exception();
        }

        public async Task PostTutorRating(string orderId, string tutorId, int rating)
        {
            Query responseQuery = db.Collection("order_rating").WhereEqualTo("OrderId", orderId).WhereEqualTo("Status", tutorId);
            QuerySnapshot responseQuerySnapshot = await responseQuery.GetSnapshotAsync();

            if (responseQuerySnapshot.Count != 0)
                throw new Exception();

            DocumentReference newRatingRef = db.Collection("totor_rating").Document();

            Dictionary<string, object> _orderResponse = new Dictionary<string, object>(){
                    {"OrderId", orderId},
                    {"CustomerId", tutorId},
                    {"Rating", rating}
                };

            await newRatingRef.SetAsync(_orderResponse);

        }

        public async Task<double> CountTutorRating(string tutorId)
        { 
            int ratingsValue = 0;

            Query ratingQuery = db.Collection("tutor_rating").WhereEqualTo("TutorId", tutorId);
            QuerySnapshot ratingQuerySnapshot = await ratingQuery.GetSnapshotAsync();

            int amountOfRatings = ratingQuerySnapshot.Count;

            return ratingsValue / amountOfRatings;

        }


    }
}

