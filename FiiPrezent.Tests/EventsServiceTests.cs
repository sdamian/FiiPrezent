using FiiPrezent.Services;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using FiiPrezent.Db;
using Xunit;

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

            result.GetParticipants().ShouldContain("Tudor");
        }

        [Fact]
        public void TryCreateEvent_WhenCodeIsUnused_ReturnsNoErrors()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("name", "descr", "code");

            result.Succeded.ShouldBe(true);
        }

        [Theory]
        [InlineData("name", "description", "cometothedarksidewehavecookies", ErrorType.CodeAlreadyExists)]
        [InlineData("name", "description", "c", ErrorType.CodeIsTooShort)]
        [InlineData("name", "", "code", ErrorType.DescriptionIsMissing)]
        [InlineData("", "description", "code", ErrorType.NameIsMissing)]
        public void TryCreateEvent_WhenOneFieldIsInvalid_ReturnsError(string name, string description, string code, ErrorType error)
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent(name, description, code);

            result.ErrorsList.ShouldContain(error);
        }

        [Fact]
        public void TryCreateEvent_WhenNameIsTooLong_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent(new string('*', 21), "description", "code");

            result.ErrorsList.ShouldContain(ErrorType.NameIsTooLong);
        }

        [Fact]
        public void TryCreateEvent_WhenNameIsMissingAndCodeIsTooShort_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("", "descr", "a");

            result.ErrorsList
                .ToArray()
                .ShouldBe(new[] { ErrorType.NameIsMissing, ErrorType.CodeIsTooShort });
        }

        [Fact]
        public void TryCreateEvent_WhenAllDataIsValid_CreateEventWithNoParticipants()
        {
            var eventsRepository = new Mock<IEventsRepository>();
            var eventsService = new EventsService(null, eventsRepository.Object, _partcipantsUpdatedNotifierMock.Object);

            var result = eventsService.TryCreateEvent("name", "description", "code");

            eventsRepository.Verify(
                    e => e.Add(It.Is<Event>(
                         x => x.Name == "name" &&
                              x.Description == "description" &&
                              x.VerificationCode == "code")
                    ));
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
            return new EventsService(null, repo, _partcipantsUpdatedNotifierMock.Object);
        }
    }
}
