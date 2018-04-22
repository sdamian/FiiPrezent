using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiiPrezent.Db
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

        public Task<Event> FindEventByVerificationCode(string verificationCode)
        {
            return Task.FromResult(_events.SingleOrDefault(x => x.VerificationCode == verificationCode));
        }

        public Task<Event> FindEventById(Guid id)
        {
            return Task.FromResult(_events.Single(x => x.Id == id));
        }
    }
}
