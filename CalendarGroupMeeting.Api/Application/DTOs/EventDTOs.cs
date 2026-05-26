namespace CalendarGroupMeeting.Api.Application.DTOs;

public record CreateEventRequest(
    string Name,
    string? Description,
    List<EventOptionDto> Options
);

public record EventOptionDto(
    DateTime StartDateTime,
    DateTime EndDateTime
);

public record CreateEventResponse(
    string EventToken
);

public record UpdateEventRequest(
    List<EventOptionDto> NewOptions
);

public record UpdateEventDescriptionInput(
    string? Description
);

public record UpdateEventResponse(
    bool Success
);
