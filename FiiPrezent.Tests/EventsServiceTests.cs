using FiiPrezent.Services;
using Shouldly;
using Xunit;

namespace FiiPrezent.Tests
{
    public class EventsServiceTests
    {
        [Fact]
        public void WhenTryingToRegisterWithAnInvalidCodeReturnsError()
        {
            var service = new EventsService();
            var result = service.RegisterParticipant("bad code", "test participant");
            result.ShouldBeNull();
        }

        [Fact]
        public void WhenTryingToRegisterWithAValidCodeReturnsSuccess()
        {
            var service = new EventsService();
            var result = service.RegisterParticipant("cometothedarksidewehavecookies", "test participant");
            result.ShouldNotBeNull();
            
        }

        [Fact]
        public void WhenTryingToRegisterWithAValidCodeAddsParticipantToEvent()
        {
            var service = new EventsService();

            string NumeParticipant = "Tudor";
            string validCode = "cometothedarksidewehavecookies";

            var result = service.RegisterParticipant(validCode, NumeParticipant);

            result.Participants.ShouldContain(NumeParticipant);

        }
    }
}
