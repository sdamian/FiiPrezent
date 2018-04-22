using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

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