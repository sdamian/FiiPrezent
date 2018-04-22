using FiiPrezent.Services;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task RegisterParticipant_WithAnInvalidCode_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = await eventsService.RegisterParticipant("bad code", "test participant", String.Empty);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task RegisterParticipant_WithAValidCode_ReturnsSuccess()
        {
            var eventsService = GetEventsService();

            var result = await eventsService.RegisterParticipant("cometothedarksidewehavecookies", "test participant", String.Empty);

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task RegisterParticipant_WithAValidCode_AddsParticipantToEvent()
        {
            var eventsService = GetEventsService();

            var result = await eventsService.RegisterParticipant("cometothedarksidewehavecookies", "Tudor", String.Empty);

            result.Participants.Select(x => x.Name).ShouldContain("Tudor");
        }

        [Fact]
        public async Task TryCreateEvent_WhenCodeIsUnused_ReturnsNoErrors()
        {
            var eventsService = GetEventsService();

            var result = await eventsService.TryCreateEvent("name", "descr", "code");

            result.Succeded.ShouldBe(true);
        }

        [Theory]
        [InlineData("name", "description", "cometothedarksidewehavecookies", ErrorType.CodeAlreadyExists)]
        [InlineData("name", "description", "c", ErrorType.CodeIsTooShort)]
        [InlineData("name", "", "code", ErrorType.DescriptionIsMissing)]
        [InlineData("", "description", "code", ErrorType.NameIsMissing)]
        public async Task TryCreateEvent_WhenOneFieldIsInvalid_ReturnsError(string name, string description, string code, ErrorType error)
        {
            var eventsService = GetEventsService();

            var result = await eventsService.TryCreateEvent(name, description, code);

            result.ErrorsList.ShouldContain(error);
        }

        [Fact]
        public async Task TryCreateEvent_WhenNameIsTooLong_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = await eventsService.TryCreateEvent(new string('*', 21), "description", "code");

            result.ErrorsList.ShouldContain(ErrorType.NameIsTooLong);
        }

        [Fact]
        public async Task TryCreateEvent_WhenNameIsMissingAndCodeIsTooShort_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = await eventsService.TryCreateEvent("", "descr", "a");

            result.ErrorsList
                .ToArray()
                .ShouldBe(new[] { ErrorType.NameIsMissing, ErrorType.CodeIsTooShort });
        }

        [Fact]
        public async Task TryCreateEvent_WhenAllDataIsValid_CreatesEventWithNoParticipants()
        {
            var eventsRepository = new Mock<IEventsRepository>();
            var eventsService = new EventsService(eventsRepository.Object, _partcipantsUpdatedNotifierMock.Object);

            await eventsService.TryCreateEvent("name", "description", "code");

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
            return new EventsService(repo, _partcipantsUpdatedNotifierMock.Object);
        }
    }
}
