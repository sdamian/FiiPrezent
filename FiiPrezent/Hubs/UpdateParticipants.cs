using System;
using System.Threading.Tasks;
using FiiPrezent.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace FiiPrezent.Hubs
{
    public class UpdateParticipants : Hub
    {
        public async Task Update(string id)
        {
            EventsService eventsService = new EventsService();
            Event @event = eventsService.FindEventById(Guid.Parse(id));

            if (@event != null)
            {
                await Groups.AddAsync(Context.ConnectionId, @event.Id.ToString());

                string json = JsonConvert.SerializeObject(@event.Participants);

                await Clients.All.SendAsync("Update", json);
                await Clients.Group(@event.Id.ToString()).SendAsync("Update", json);
            }
        }
    }
}