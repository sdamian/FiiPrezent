using System;
using System.Collections.Generic;
using System.Linq;

namespace FiiPrezent.Services
{
    public class EventsService
    {
        private readonly IParticipantsUpdatedNotifier _participantsUpdatedNotifier;

        private static readonly List<Event> Events = new List<Event>
        {
            new Event
            {
                Id = Guid.Parse("13db9c8b-39f6-41fb-b449-0d4ba6f9f273"),
                Name = "Best training ever",
                Description = "Modern web application development with ASP.NET Core :o",
                VerificationCode = "cometothedarksidewehavecookies"
            }
        };

        public EventsService(IParticipantsUpdatedNotifier participantsUpdatedNotifier)
        {
            _participantsUpdatedNotifier = participantsUpdatedNotifier;
        }

        public Event RegisterParticipant(string verificationCode, string participantName)
        {
            var @event = FindEventByVerificationCode(verificationCode);
            if (@event == null)
            {
                return null;
            }

            @event.RegisterParticipant(participantName);
            _participantsUpdatedNotifier.OnParticipantsUpdated(@event.Id, @event.Participants);

            return @event;
        }


        public Event FindEventByVerificationCode(string verificationCode)
        {
            return Events.SingleOrDefault(x => x.VerificationCode == verificationCode);
        }

        public Event FindEventById(Guid id)
        {
            return Events.Single(x => x.Id == id);
        }
    }
}
