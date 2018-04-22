using System;
using FiiPrezent.Db;

namespace FiiPrezent.Services
{
    public interface IEventsRepository
    {
        void Add(Event @event);
        void Update(Event @event);
        Event FindEventByVerificationCode(string verificationCode);
        Event FindEventById(Guid id);
        void Delete(Guid id);
    }
}
