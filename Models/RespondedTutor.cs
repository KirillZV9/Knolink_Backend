using PomogatorAPI.Models;

namespace WebApi.Models
{
    public class RespondedTutor
    {
        public Tutor Tutor { get; set; }
        public string Price { get; set; }

        public RespondedTutor(Tutor tutor, string price)
        {
            Tutor = tutor;
            Price = price;
        }
    }
}
