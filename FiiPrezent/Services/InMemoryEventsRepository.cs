using System;
using System.Collections.Generic;
using System.Linq;
using FiiPrezent.Db;

namespace FiiPrezent.Services
{
    public class InMemoryEventsRepository : IEventsRepository
    {
        private readonly List<Event> _events = new List<Event>();

        public InMemoryEventsRepository(IEnumerable<Event> events)
        {
            _events.AddRange(events);
        }

        public void Add(Event @event)
        {
            _events.Add(@event);
        }

        public void Update(Event @event)
        {
            throw new NotImplementedException();
        }

        public Event FindEventByVerificationCode(string verificationCode)
        {
            return _events.SingleOrDefault(x => x.VerificationCode == verificationCode);
        }

        public Event FindEventById(Guid id)
        {
            return _events.Single(x => x.Id == id);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
