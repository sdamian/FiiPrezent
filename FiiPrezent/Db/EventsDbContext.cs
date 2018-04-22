using FiiPrezent.Models;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiiPrezent.Db
{
    public class EventsDbContext: IdentityDbContext<ApplicationUser>
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
    }
}
