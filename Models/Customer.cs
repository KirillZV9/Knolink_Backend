namespace PomogatorAPI.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TelNum { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }

        public Customer(string id, string telNum, string name)
        {
            Id = id;
            Name = name;
            TelNum = telNum;
            Balance = 0;
            Status = "делаеца";
        }
    }

}
