using CalendarGroupMeeting.Api.Application.DTOs;

namespace CalendarGroupMeeting.Api.Application.Services;

public interface IEventService
{
    Task<CreateEventResponse> CreateEventAsync(CreateEventRequest request);
    Task<GetEventResponse?> GetEventAsync(string token);
    Task<CheckNameResponse> CheckNameExistsAsync(string eventToken, string name);
    Task<EventResultResponse?> GetEventResultsAsync(string token);
    Task<UpdateEventResponse> UpdateEventAsync(string token, UpdateEventRequest request);
    Task<bool> DeleteEventOptionAsync(string eventToken, Guid optionId);
    Task<GetUserResponseResponse> GetUserResponseAsync(string eventToken, string name);
    Task<UpdateEventResponse> UpdateEventDescriptionAsync(string token, UpdateEventDescriptionInput request);
}
