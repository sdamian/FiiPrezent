using System;
using System.Linq;
using FiiPrezent.Services;
using Microsoft.EntityFrameworkCore;

namespace FiiPrezent.Db
{
    public class DbEventsRepository : IEventsRepository, IDisposable
    {
        private readonly EventsDbContext _context;

        public DbEventsRepository(EventsDbContext context)
        {
            _context = context;
        }

        public void Add(Event @event)
        {
            _context.Add(@event);
        }

        public void Update(Event @event)
        {
            _context.Events.Update(@event);
        }

        public Event FindEventByVerificationCode(string verificationCode)
        {
            return _context.Events.SingleOrDefault(x => x.VerificationCode == verificationCode);
        }

        public Event FindEventById(Guid id)
        {
            return _context.Events.Include(x => x.Participants).SingleOrDefault(x => x.Id == id);
        }

        public void Delete(Guid id)
        {
            _context.Remove(FindEventById(id));
        }

        public void Dispose()
        {
            _context.SaveChanges();
        }
    }
}
