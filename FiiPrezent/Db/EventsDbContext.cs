using Microsoft.EntityFrameworkCore;

namespace FiiPrezent.Db
{
    public class EventsDbContext: DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
    }
}
