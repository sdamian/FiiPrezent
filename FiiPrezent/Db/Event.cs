using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FiiPrezent.Db
{
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            Participants = new List<Participant>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        public ICollection<Participant> Participants { get; set; }

        public void RegisterParticipant(string name, string photoUrl)
        {
            Participants.Add(new Participant
            {
                Name = name,
                PhotoUrl = photoUrl
            });
        }

    }
}
