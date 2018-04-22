using System;
using System.Threading.Tasks;
using FiiPrezent.Models;

namespace FiiPrezent.Services
{
    public interface IParticipantsUpdatedNotifier
    {
        Task OnParticipantsUpdated(Guid eventId, EventParticipant[] newParticipants);
    }
}
