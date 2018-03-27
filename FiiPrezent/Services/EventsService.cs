using System;
using System.Collections.Generic;
using System.Linq;
using FiiPrezent.Controllers;
using FiiPrezent.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FiiPrezent.Services
{
    public class EventsService
    {
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
        
        private readonly IHubContext<UpdateParticipants> _hubContext;

        public EventsService(IHubContext<UpdateParticipants> hubContext)
        {
            _hubContext = hubContext;
        }

        public Event RegisterParticipant(string verificationCode, string participantName)
        {
            var @event = FindEventByVerificationCode(verificationCode);
            if (@event == null) 
            {
                return null;
            }

            @event.RegisterParticipant(participantName);
            var group = _hubContext.Clients.Group(@event.SignalrGroup);
            group.SendAsync("Update", @event.Participants, null); // the null is a stupid hack to pass the participants array as one param
       
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
