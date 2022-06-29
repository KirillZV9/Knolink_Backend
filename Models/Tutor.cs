using Google.Cloud.Firestore;

namespace PomogatorAPI.Models
{
    [FirestoreData]
    public class Tutor
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;
        [FirestoreProperty]
        public string TelNum { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Name { get; set; } = string.Empty;
        [FirestoreProperty]
        public double Balance { get; set; }
        [FirestoreProperty]
        public double Rating { get; set; }
        [FirestoreProperty]
        public string? University { get; set; }

        public Tutor(string id, string telNum, string name, string? university)
        {
            Id = id;
            TelNum = telNum;
            Name = name;
            Balance = 0;
            Rating = 0;
            University = university;
        }

        public Tutor() { }
    }   
}
