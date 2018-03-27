using System;
using System.Threading.Tasks;
using FiiPrezent.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace FiiPrezent.Hubs
{
    public class UpdateParticipants : Hub
    {
        public async Task JoinEventRoom(string eventId)
        {
            await Groups.AddAsync(Context.ConnectionId, eventId);
        }
    }
}