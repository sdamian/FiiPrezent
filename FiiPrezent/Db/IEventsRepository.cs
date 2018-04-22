using System;
using System.Threading.Tasks;

namespace FiiPrezent.Db
{
    public interface IEventsRepository
    {
        void Add(Event @event);
        Task<Event> FindEventByVerificationCode(string verificationCode);
        Task<Event> FindEventById(Guid id);
    }
}
