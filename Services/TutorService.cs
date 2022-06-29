namespace PomogatorAPI.Repositories

{
    using Google.Cloud.Firestore;
    using PomogatorAPI.Interfaces;
    using PomogatorAPI.Models;
    using static Config;


    public class TutorService : ITutor
    {
        public List<Tutor> Tutors { get; set; }

        private readonly FirestoreDb db = FirestoreDb.Create(fbProjectId);

        private const string fbCollection = "tutors";

        public TutorService()
        {
            Tutors = new List<Tutor>();
        }




        public async Task PostAsync(string id, string telNum, string name, string? university)
        {
            Tutor tutor = new Tutor(id, telNum, name, university);

            if (DoesTutorExistAsync(tutor.Id).Result)
                throw new ArgumentException(); 

            DocumentReference docRef = db.Collection(fbCollection).Document(tutor.Id);
            Dictionary<string, object> _tutor = new Dictionary<string, object>()
            {
                    {"Id", tutor.Id},
                    {"Name", tutor.Name},
                    {"TelNum", tutor.TelNum },
                    {"Balance", tutor.Balance },
                    {"Rating", tutor.Rating },
                    {"University", tutor.University }
            };
        await docRef.SetAsync(_tutor);
        }

        public async Task<bool> DoesTutorExistAsync(string Id)
        {
            Query docRef = db.Collection(fbCollection).WhereEqualTo("Id", Id).Limit(1);
            QuerySnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Count > 0) 
                return true;
            return false;
        }

        public async Task DeleteAsync(string id)
        {
            if (!DoesTutorExistAsync(id).Result)
                throw new ArgumentException();
            DocumentReference docRef = db.Collection(fbCollection).Document(id);
            await docRef.DeleteAsync();
        }

        public async Task UpdateAsync(Tutor tutor)
        {

            if (!DoesTutorExistAsync(tutor.Id).Result)
                throw new ArgumentException();

            DocumentReference docRef = db.Collection(fbCollection).Document(tutor.Id);
            Dictionary<string, object> _tutor = new Dictionary<string, object>()
            {
                    {"Id", tutor.Id},
                    {"Name", tutor.Name},
                    {"TelNum", tutor.TelNum },
                    {"Balance", tutor.Balance },
                    {"Rating", tutor.Rating },
                    {"University", tutor.University }
            };

        await docRef.UpdateAsync(_tutor);
        }

        public async Task GetAsync(string id)
        {
            if (!DoesTutorExistAsync(id).Result)
                throw new ArgumentException();

            DocumentReference docRef = db.Collection(fbCollection).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            Tutor tutor = snapshot.ConvertTo<Tutor>();
            Tutors.Add(tutor);

        }

        public async Task GetTutorsById(Dictionary<string, string> responsesDict)
        {

            foreach(var response in responsesDict)
            {
                await GetAsync(response.Key);
            }
        }

        public async Task UpdateBalance(string tutorId, string price)
        {
            DocumentReference tutorRef = db.Collection(fbCollection).Document(tutorId);
            DocumentSnapshot tutorSnapshot = await tutorRef.GetSnapshotAsync();

            double newBalance = tutorSnapshot.GetValue<double>("Balance")
                + double.Parse(price);

            await tutorRef.UpdateAsync("Balance", newBalance);
        }


    }


}