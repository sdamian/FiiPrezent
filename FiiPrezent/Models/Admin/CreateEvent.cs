using FiiPrezent.Db;

namespace FiiPrezent.Models.Admin
{
    public class CreateEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SecretCode { get; set; }
    }
}
