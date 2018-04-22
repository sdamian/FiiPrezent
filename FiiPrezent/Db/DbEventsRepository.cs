using System;
using FiiPrezent.Services;

namespace FiiPrezent.Db
{
    class DbEventsRepository : IEventsRepository
    {
        // TODO: get data from DB

        public void Add(Event @event)
        {
            throw new NotImplementedException();
        }

        public Event FindEventByVerificationCode(string verificationCode)
        {
            throw new NotImplementedException();
        }

        public Event FindEventById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
