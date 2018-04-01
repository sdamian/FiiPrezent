using System;
using System.Collections.Generic;

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
            var @event = _eventsRepo.FindEventByVerificationCode(code);

            CreatedResult result = new CreatedResult();

            if (@event != null)
                result.AddError(ErrorType.CodeAlreadyExists);

            if (string.IsNullOrEmpty(name))
                result.AddError(ErrorType.NameIsMissing);

            if (name.Length > 20)
                result.AddError(ErrorType.NameIsTooLong);

            if (code.Length < 4)
                result.AddError(ErrorType.CodeIsTooShort);

            if (string.IsNullOrEmpty(descr))
                result.AddError(ErrorType.DescriptionIsMissing);

            @event = new Event
            {
                Name = name,
                Description = descr,
                VerificationCode = code,
            };

            _eventsRepo.Add(@event);

            return result;
        }

        public class CreatedResult
        {
            public CreatedResult()
            {
                ErrorsList = new List<ErrorType>();
            }

            public void AddError(ErrorType error)
            {
                ErrorsList.Add(error);
            }

            public CreatedResult(ErrorType errorType)
            {
                ErrorsList = new List<ErrorType>
                {
                    errorType
                };
            }

            public bool Succeded
            {
                get
                {
                    return ErrorsList.Count == 0;
                }
            }

            public Guid EventId { get; set; }
            public List<ErrorType> ErrorsList { get; set; }
        }
    }

    public enum ErrorType
    {
        CodeAlreadyExists,
        NameIsMissing,
        NameIsTooLong,
        DescriptionIsMissing,
        CodeIsTooShort
    }
}