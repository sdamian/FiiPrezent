﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiiPrezent.Db;
using FiiPrezent.Models;

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

        public async Task<Event> RegisterParticipant(string verificationCode, string participantName, string photoUrl)
        {
            var @event = await _eventsRepo.FindEventByVerificationCode(verificationCode);
            if (@event == null)
            {
                return null;
            }

            @event.RegisterParticipant(participantName, photoUrl);
            var newParticipants = @event.Participants
                .Select(x => new EventParticipant(x.Name, x.PhotoUrl))
                .ToArray();

            await _participantsUpdatedNotifier.OnParticipantsUpdated(@event.Id, newParticipants);

            return @event;
        }

        public async Task<CreatedResult> TryCreateEvent(string name, string descr, string code)
        {
            var @event = await _eventsRepo.FindEventByVerificationCode(code);

            CreatedResult result = new CreatedResult();

            if (@event != null)
                result.AddError(ErrorType.CodeAlreadyExists);

            if (string.IsNullOrEmpty(name))
                result.AddError(ErrorType.NameIsMissing);

            if (string.IsNullOrEmpty(descr))
                result.AddError(ErrorType.DescriptionIsMissing);

            if (name.Length > 20)
                result.AddError(ErrorType.NameIsTooLong);

            if (code.Length < 4)
                result.AddError(ErrorType.CodeIsTooShort);

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

            public bool Succeded => ErrorsList.Count == 0;

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
