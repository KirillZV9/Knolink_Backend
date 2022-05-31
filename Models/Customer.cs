using Google.Cloud.Firestore;

namespace PomogatorAPI.Models
{
    [FirestoreData]
    public class Customer
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string TelNum { get; set; }
        [FirestoreProperty]
        public double Balance { get; set; }


        public Customer(string id, string telNum, string name)
        {
            Id = id;
            Name = name;
            TelNum = telNum;
            Balance = 0;
        }

        public Customer() { }
    }

}
