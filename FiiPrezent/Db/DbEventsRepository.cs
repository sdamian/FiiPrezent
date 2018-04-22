using System;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<Event> FindEventByVerificationCode(string verificationCode)
        {
            return _db.Events
                .Include(x => x.Participants)
                .SingleOrDefaultAsync(x => x.VerificationCode == verificationCode);
        }

        public Task<Event> FindEventById(Guid id)
        {
            return _db.Events
                .Include(x => x.Participants)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Dispose()
        {
            _db.SaveChanges();
        }
    }
}
