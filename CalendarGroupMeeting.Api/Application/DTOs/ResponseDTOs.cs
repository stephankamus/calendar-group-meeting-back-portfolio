namespace CalendarGroupMeeting.Api.Application.DTOs;

public record CreateResponseRequest(
    string EventToken,
    string Name,
    List<Guid> SelectedOptionIds
);

public record CreateResponseResponse(
    string ResponseToken
);

public record UpdateResponseRequest(
    List<Guid> SelectedOptionIds
);
