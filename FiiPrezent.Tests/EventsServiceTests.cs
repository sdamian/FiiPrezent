using System;
using System.Collections.Generic;
using FiiPrezent.Services;
using Shouldly;
using Xunit;
using Moq;

namespace FiiPrezent.Tests
{
    public class EventsServiceTests
    {
        private readonly Mock<IParticipantsUpdatedNotifier> _partcipantsUpdatedNotifierMock;

        public EventsServiceTests()
        {
            _partcipantsUpdatedNotifierMock = new Mock<IParticipantsUpdatedNotifier>();
        }

        [Fact]
        public void RegisterParticipant_WithAnInvalidCode_ReturnsError()
        {
            var eventsService = GetEventsService();
            var result = eventsService.RegisterParticipant("bad code", "test participant");

            result.ShouldBeNull();
        }

        [Fact]
        public void RegisterParticipant_WithAValidCode_ReturnsSuccess()
        {
            var eventsService = GetEventsService();
            var result = eventsService.RegisterParticipant("cometothedarksidewehavecookies", "test participant");

            result.ShouldNotBeNull();
        }

        [Fact]
        public void RegisterParticipant_WithAValidCode_AddsParticipantToEvent()
        {
            var eventsService = GetEventsService();
            var result = eventsService.RegisterParticipant("cometothedarksidewehavecookies", "Tudor");

            result.Participants.ShouldContain("Tudor");
        }

        [Fact(Skip = "TODO")]
        public void TryCreateEvent_WhenCodeIsUnused_ReturnsNoErrors()
        {
            var eventsService = GetEventsService();
            var result = eventsService.TryCreateEvent("name", "descr", "code");

//            result.EventId.ShouldNotBeNull();
//            result.Errors.ShouldBeEmpty();
        }

        [Fact(Skip = "TODO")]
        public void TryCreateEvent_WhenCodeIsUsed_ReturnsError()
        {
            var eventsService = GetEventsService();
            var result = eventsService.TryCreateEvent("name", "descr", "code");

//            result.Errors.ShouldHaveSingleItem();
//            result.Errors[0].ShouldBe("Code already exists");
        }

        private EventsService GetEventsService(IEnumerable<Event> initialEvents = null)
        {
            var repo = new InMemoryEventsRepository(initialEvents ?? new[]
            {
                new Event
                {
                    Id = Guid.Parse("13db9c8b-39f6-41fb-b449-0d4ba6f9f273"),
                    Name = "Best training ever",
                    Description = "Modern web application development with ASP.NET Core :o",
                    VerificationCode = "cometothedarksidewehavecookies"
                }
            });
            return new EventsService(repo, _partcipantsUpdatedNotifierMock.Object);
        }
    }
}
