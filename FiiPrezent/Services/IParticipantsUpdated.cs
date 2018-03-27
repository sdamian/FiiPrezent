using System;

namespace FiiPrezent.Services
{
    public interface IParticipantsUpdated
    {
        void OnParticipantsUpdated(Guid eventId, string[] newParticipants);
    }
}