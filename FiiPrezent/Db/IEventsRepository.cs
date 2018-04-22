using System;

namespace FiiPrezent.Db
{
    public interface IEventsRepository
    {
        void Add(Event @event);
        Event FindEventByVerificationCode(string verificationCode);
        Event FindEventById(Guid id);
    }
}
