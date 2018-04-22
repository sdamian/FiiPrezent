using System;

namespace FiiPrezent.Db
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public string Name { get; set; }        
        public string PhotoUrl { get; set; }
    }
}