using System;

namespace FiiPrezent.Services
{
    public class EventsService
    {
        private readonly IEventsRepository _eventsRepo;
        private readonly IParticipantsUpdatedNotifier _participantsUpdatedNotifier;

        public EventsService(
            IEventsRepository eventsRepo,
            IParticipantsUpdatedNotifier participantsUpdatedNotifier)
        {
            _eventsRepo = eventsRepo;
            _participantsUpdatedNotifier = participantsUpdatedNotifier;
        }

        public Event RegisterParticipant(string verificationCode, string participantName)
        {
            var @event = _eventsRepo.FindEventByVerificationCode(verificationCode);
            if (@event == null)
            {
                return null;
            }

            @event.RegisterParticipant(participantName);
            _participantsUpdatedNotifier.OnParticipantsUpdated(@event.Id, @event.Participants);

            return @event;
        }

        public CreatedResult TryCreateEvent(string name, string descr, string code)
        {
            var @event = new Event
            {
                Name = name,
                Description = descr,
                VerificationCode = code,
            };
            _eventsRepo.Add(@event);

            return new CreatedResult(@event.Id);
        }

        public class CreatedResult
        {
            public CreatedResult(Guid? eventId, string[] errors = null)
            {
                EventId = eventId;
                Errors = errors ?? new string[0];
            }

            public Guid? EventId { get; set; }
            public string[] Errors { get; set; }
        }
    }
}
