using System;
using System.Collections.Generic;
using System.Linq;
using FiiPrezent.Controllers;

namespace FiiPrezent.Services
{
    public class EventsService
    {
        private readonly IParticipantsUpdated _participantsUpdated;

        private static readonly List<Event> _events = new List<Event>
        {
            new Event
            {
                Id = Guid.Parse("13db9c8b-39f6-41fb-b449-0d4ba6f9f273"),
                Name = "Best training ever",
                Description = "Modern web application development with ASP.NET Core :o",
                VerificationCode = "cometothedarksidewehavecookies"
            }
        };

        public EventsService(IParticipantsUpdated participantsUpdated)
        {
            _participantsUpdated = participantsUpdated;
        }

        public Event RegisterParticipant(string verificationCode, string participantName)
        {
            var @event = FindEventByVerificationCode(verificationCode);
            if (@event == null)
            {
                return null;
            }

            @event.RegisterParticipant(participantName);
            _participantsUpdated.OnParticipantsUpdated(@event.Id, @event.Participants);
            return @event;
        }


        public Event FindEventByVerificationCode(string verificationCode)
        {
            return _events.SingleOrDefault(x => x.VerificationCode == verificationCode);
        }

        public Event FindEventById(Guid id)
        {
            return _events.Single(x => x.Id == id);
        }
    }
}
