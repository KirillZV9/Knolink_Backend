using PomogatorAPI.Models;

namespace PomogatorAPI.Interfaces
{
    public interface ITutor
    {
        Tutor Get(int id);
        bool DoesTutorExist(int id);
        void Post(Tutor tuor);
        void Update(Tutor tutor);
        void Delete(int id);
    }
}
