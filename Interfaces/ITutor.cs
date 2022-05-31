using PomogatorAPI.Models;

namespace PomogatorAPI.Interfaces
{
    public interface ITutor
    {
        List<Tutor> Tutors { get; set; }
        Task GetAsync(string id);
        Task<bool> DoesTutorExistAsync(string id);
        Task PostAsync(string id, string telNum, string name, string university);
        Task UpdateAsync(Tutor tutor);
        Task DeleteAsync(string id);
    }
}
