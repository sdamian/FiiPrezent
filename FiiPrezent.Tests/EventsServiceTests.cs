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
        
        [Fact]
        public void TryCreateEvent_WhenCodeIsUnused_ReturnsNoErrors()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("name", "descr", "code");

            result.Succeded.ShouldBe(true);
        }
        
        [Fact]
        public void TryCreateEvent_WhenCodeIsUsed_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("name", "descr", "cometothedarksidewehavecookies");
            
            result.ErrorsList.ShouldContain(ErrorType.CodeAlreadyExists);
        }

        [Fact]
        public void RegisterParticipant_WithAllIsValid_CreateEventWithNoParticipants()
        {
            var eventRepo = new Mock<IEventsRepository>();
            var eventsService = new EventsService(eventRepo.Object, _partcipantsUpdatedNotifierMock.Object);


            var result = eventsService.TryCreateEvent("name", "description", "code");

            eventRepo.Verify(e => 
                e.Add(It.Is<Event>(x => 
                                    x.Name == "name" && 
                                    x.Description == "description" && 
                                    x.VerificationCode == "code")));
        }

        [Fact]
        public void TryCreateEvent_WhenCodeIsTooShort_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("name", "descr", "a");

            result.ErrorsList.ShouldContain(ErrorType.CodeIsTooShort);
        }

        [Fact]
        public void TryCreateEvent_WhenNameIsMissingAndCodeIsTooShort_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("", "descr", "a");

         
            result.ErrorsList
                .ToArray()
                .ShouldBe(new [] { ErrorType.NameIsMissing, ErrorType.CodeIsTooShort});

        }

        [Fact]
        public void TryCreateEvent_WhenNameIsMissing_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("", "descr", "code");

            result.ErrorsList.ShouldContain(ErrorType.NameIsMissing);
        }

        [Fact]
        public void TryCreateEvent_WhenNameIsTooLong_ReturnsError()
        {
            var eventsService = GetEventsService();

            string name = "";
            for(int i = 0; i < 30; i++)
            {
                name += i.ToString();
            }

            var result = eventsService.TryCreateEvent(name, "descr", "code");

            result.ErrorsList.ShouldContain(ErrorType.NameIsTooLong);
        }

        [Fact]
        public void TryCreateEvent_WhenDescriptionIsMissing_ReturnsError()
        {
            var eventsService = GetEventsService();

            var result = eventsService.TryCreateEvent("name", "", "code");

            result.ErrorsList.ShouldContain(ErrorType.DescriptionIsMissing);
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
