using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FiiPrezent.Db
{
    public class DbEventsRepository : IEventsRepository, IDisposable
    {
        private readonly EventsDbContext _db;

        public DbEventsRepository(EventsDbContext db)
        {
            _db = db;
        }

        public void Add(Event @event)
        {
            _db.Add(@event);
        }

        public Event FindEventByVerificationCode(string verificationCode)
        {
            return _db.Events.Include(x => x.Participants).SingleOrDefault(x => x.VerificationCode == verificationCode);
        }

        public Event FindEventById(Guid id)
        {
            return _db.Events.Include(x => x.Participants).SingleOrDefault(x => x.Id == id);
        }

        public void Dispose()
        {
            _db.SaveChanges();
        }
    }
}
