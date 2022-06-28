using Google.Cloud.Firestore;

namespace PomogatorAPI.Models
{
    [FirestoreData]
    public class Customer
    {
        [FirestoreProperty]
        public string Id { get; set; } = String.Empty;
        [FirestoreProperty]
        public string Name { get; set; } = String.Empty;
        [FirestoreProperty]
        public string TelNum { get; set; } = String.Empty;


        public Customer(string id, string telNum, string name)
        {
            Id = id;
            Name = name;
            TelNum = telNum;
        }

        public Customer() { }
    }

}
