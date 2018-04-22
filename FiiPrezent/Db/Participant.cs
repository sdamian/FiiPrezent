using System;

namespace FiiPrezent.Db
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }

        public string Name { get; set; }

       // [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }
    }
}