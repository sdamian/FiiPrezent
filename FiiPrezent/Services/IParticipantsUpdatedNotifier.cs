using System;

namespace FiiPrezent.Services
{
    public interface IParticipantsUpdatedNotifier
    {
        void OnParticipantsUpdated(Guid eventId, string[] newParticipants);
    }
}