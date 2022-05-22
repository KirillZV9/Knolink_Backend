using Google.Cloud.Firestore;

namespace PomogatorAPI.Models
{
    [FirestoreData]
    public class Tutor
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string TelNum { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public double Balance { get; set; }
        [FirestoreProperty]
        public double? Rating { get; set; }
        [FirestoreProperty]
        public string? University { get; set; }
        [FirestoreProperty]
        public string Status { get; set; }

        public Tutor(string id, string telNum, string name)
        {
            Id = id;
            TelNum = telNum;
            Name = name;
            Balance = 0;
            Rating = null;
            Status = "Creating";
        }

        public Tutor() { }
    }   
}
/*
Дата рождения?
*/