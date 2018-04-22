using System.Linq;
using FiiPrezent.Db;

namespace FiiPrezent.Models
{
    public class EventViewModel
    {
        public EventViewModel(Event @event)
        {
            Id = @event.Id.ToString();
            Name = @event.Name;
            Description = @event.Description;
            Participants = @event.Participants
                .Select(x => new EventParticipant(x.Name, x.PhotoUrl))
                .ToArray();
        }

        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public EventParticipant[] Participants { get; }
    }

}
