

namespace PomogatorAPI.Repositories
{
    using PomogatorAPI.Models;
    using PomogatorAPI.Interfaces;
    using static Config;
    using Google.Cloud.Firestore;

    public class OrderService : IOrder
    {
        public Order Order { get; set; } = new Order();

        public List<Order> OrdersList { get; set; } = new List<Order>();

        public Dictionary<string, string> RespondedTutors { get; set; } = new Dictionary<string, string>();

        private readonly FirestoreDb db = FirestoreDb.Create(fbProjectId);

        private const string fbCollection = "orders";

        private double TutorRating;

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
                Order.Id = snapshot.Id;
            }
            else
                throw new Exception();
        }

        public async Task GetOrdersById(string cutomerId, string atribute)
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo(atribute, cutomerId);
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
                    order.Id = snapshot.Id;
                    OrdersList.Add(order);
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
                await docRef.UpdateAsync("Closed", DateTime.UtcNow);
            }
            else
                throw new Exception();
        }

        public async Task PostResponse(string orderId, string tutorId, string price)
        {
            Query responseQuery = db.Collection("order_response").WhereEqualTo("OrderId", orderId).WhereEqualTo("TutorId", tutorId);
            QuerySnapshot responseQuerySnapshot = await responseQuery.GetSnapshotAsync();

            if (responseQuerySnapshot.Count != 0)
                throw new Exception();

            DocumentReference docRef = db.Collection("order_response").Document();

            Dictionary<string, object> _orderResponse = new Dictionary<string, object>(){
                    {"OrderId", orderId},
                    {"TutorId", tutorId},
                    {"Price", price}
                };

            await docRef.SetAsync(_orderResponse);
        }

        public async Task<Dictionary<string, string>> GetAllRespondedTutors(string orderId)
        {
            Query ordersQuery = db.Collection("order_response").WhereEqualTo("OrderId", orderId);
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();

            foreach (DocumentSnapshot snapshot in ordersQuerySnapshot.Documents)
            {
                RespondedTutors.Add(snapshot.GetValue<string>("TutorId"), snapshot.GetValue<string>("Price"));
            }

            return RespondedTutors;
           
        }

        public async Task PostTutorRating(string orderId, int rating)
        {
            Query responseQuery = db.Collection("order_rating").WhereEqualTo("OrderId", orderId);
            QuerySnapshot responseQuerySnapshot = await responseQuery.GetSnapshotAsync();

            if (responseQuerySnapshot.Count != 0)
                throw new Exception();

            DocumentReference orderRef = db.Collection(fbCollection).Document(orderId);
            DocumentSnapshot orderSnapshot = await orderRef.GetSnapshotAsync();

            DocumentReference newRatingRef = db.Collection("tutor_rating").Document();

            string tutorId = orderSnapshot.GetValue<string>("TutorId");

            Dictionary<string, object> _tutorRating = new Dictionary<string, object>(){
                    {"OrderId", orderId},
                    {"TutorId", tutorId},
                    {"Rating", rating}
                };

            await newRatingRef.SetAsync(_tutorRating);

            await CountTutorRating(tutorId);

            await UpdateRatingAsync(tutorId, TutorRating);


        }

        public async Task CountTutorRating(string tutorId)
        { 
            int ratingsValue = 0;

            Query ratingQuery = db.Collection("tutor_rating").WhereEqualTo("TutorId", tutorId);
            QuerySnapshot ratingQuerySnapshot = await ratingQuery.GetSnapshotAsync();

            foreach(DocumentSnapshot ratingSnapshot in ratingQuerySnapshot.Documents)
            {
                ratingsValue += int.Parse(ratingSnapshot.GetValue<string>("Rating"));
            }

            int amountOfRatings = ratingQuerySnapshot.Count;

            TutorRating =  ratingsValue / amountOfRatings;

        }

        public async Task UpdateRatingAsync(string id, double rating)
        {
            DocumentReference tutorRef = db.Collection("tutors").Document(id);
            
            if (tutorRef != null)
                await tutorRef.UpdateAsync("Rating", rating);
        }

        public async Task GetAllOrders()
        {
            Query ordersQuery = db.Collection(fbCollection).WhereEqualTo("Status", "open");
            QuerySnapshot ordersQuerySnapshot = await ordersQuery.GetSnapshotAsync();
            SetOrdersDict(ordersQuerySnapshot);
        }


    }
}

