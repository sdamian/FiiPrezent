using System;
using System.Threading.Tasks;

namespace FiiPrezent.Services
{
    public interface IParticipantsUpdatedNotifier
    {
        Task OnParticipantsUpdated(Guid eventId, string[] newParticipants);
    }
}
