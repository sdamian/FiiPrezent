using System;
using FiiPrezent.Db;

namespace FiiPrezent.Services
{
    public interface IEventsRepository
    {
        void Add(Event @event);
        Event FindEventByVerificationCode(string verificationCode);
        Event FindEventById(Guid id);
    }
}
