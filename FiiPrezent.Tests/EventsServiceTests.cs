using FiiPrezent.Services;
using Shouldly;
using Xunit;

namespace FiiPrezent.Tests
{
    public class EventsServiceTests
    {
        private readonly EventsService _service;

        public EventsServiceTests()
        {
            _service = new EventsService();
        }

        [Fact]
        public void RegisterParticipant_WithAnInvalidCode_ReturnsError()
        {
            var result = _service.RegisterParticipant("bad code", "test participant");

            result.ShouldBeNull();
        }

        [Fact]
        public void RegisterParticipant_WithAValidCode_ReturnsSuccess()
        {
            var result = _service.RegisterParticipant("cometothedarksidewehavecookies", "test participant");

            result.ShouldNotBeNull();
        }

        [Fact]
        public void RegisterParticipant_WithAValidCode_AddsParticipantToEvent()
        {
            var result = _service.RegisterParticipant("cometothedarksidewehavecookies", "Tudor");

            result.Participants.ShouldContain("Tudor");
        }
    }
}
