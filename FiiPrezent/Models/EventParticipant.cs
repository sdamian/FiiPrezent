namespace FiiPrezent.Models
{
    public class EventParticipant
    {
        public EventParticipant(string name, string photoUrl)
        {
            Name = name;
            PhotoUrl = photoUrl;
        }

        public string Name { get; }
        public string PhotoUrl { get; }
    }
}
