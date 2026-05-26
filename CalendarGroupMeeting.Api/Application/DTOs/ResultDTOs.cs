namespace CalendarGroupMeeting.Api.Application.DTOs;

public record EventResultResponse(
    List<OptionResultDto> Results
);

public record OptionResultDto(
    Guid OptionId,
    int Votes,
    List<string> Names
);
