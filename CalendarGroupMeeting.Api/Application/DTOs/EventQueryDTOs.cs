namespace CalendarGroupMeeting.Api.Application.DTOs;

public record GetEventResponse(
    string Name,
    string? Description,
    List<EventOptionResponse> Options
);

public record EventOptionResponse(
    Guid Id,
    DateTime StartDateTime,
    DateTime EndDateTime
);

public record CheckNameResponse(
    bool Exists
);

public record GetUserResponseRequest(
    string EventToken,
    string Name
);

public record GetUserResponseResponse(
    bool Exists,
    string? ResponseToken,
    List<Guid> SelectedOptionIds
);
