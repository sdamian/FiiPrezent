using System;
using System.Threading.Tasks;
using FiiPrezent.Hubs;
using FiiPrezent.Models;
using Microsoft.AspNetCore.SignalR;

namespace FiiPrezent.Services
{
    class ParticipantsUpdatedNotifier : IParticipantsUpdatedNotifier
    {
        private readonly IHubContext<UpdateParticipants> _hubContext;

        public ParticipantsUpdatedNotifier(IHubContext<UpdateParticipants> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task OnParticipantsUpdated(Guid eventId, EventParticipant[] newParticipants)
        {
            var group = _hubContext.Clients.Group(eventId.ToString());
            // the null is a stupid hack to pass the participants array as one param
            await group.SendAsync("Update", newParticipants, null);
        }

    }
}
