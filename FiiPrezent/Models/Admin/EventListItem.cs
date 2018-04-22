using FiiPrezent.Db;

namespace FiiPrezent.Models.Admin
{
    public class EventListItem
    {
        public EventListItem(Event @event)
        {
            Id = @event.Id.ToString();
            Name = @event.Name;
            Description = @event.Description;
            SecretCode = @event.VerificationCode;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SecretCode { get; set; }
    }
}
