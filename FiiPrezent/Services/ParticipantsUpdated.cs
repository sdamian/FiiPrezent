using System;
using FiiPrezent.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FiiPrezent.Services
{
    class ParticipantsUpdated : IParticipantsUpdated
    {
        private readonly IHubContext<UpdateParticipants> _hubContext;

        public ParticipantsUpdated(IHubContext<UpdateParticipants> hubContext)
        {
            _hubContext = hubContext;
        }

        public void OnParticipantsUpdated(Guid eventId, string[] newParticipants)
        {
            var group = _hubContext.Clients.Group(eventId.ToString());
            // the null is a stupid hack to pass the participants array as one param
            group.SendAsync("Update", newParticipants, null);
        }

    }
}
