namespace PomogatorAPI.Models
{
    public class Tutor
    {
        public string Id { get; set; }

        public string TelNum { get; set; }

        public string Name { get; set; }
        public decimal Balance { get; set; }
        public double? Rating { get; set; }
        public string Status { get; set; }

        public Tutor(string id, string telNum, string name)
        {
            Id = id;
            TelNum = telNum;
            Name = name;
            Balance = 0;
            Rating = null;
            Status = "делаеца";
        }
    }
}
