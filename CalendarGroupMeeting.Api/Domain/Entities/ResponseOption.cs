namespace CalendarGroupMeeting.Api.Domain.Entities;

public class ResponseOption
{
    public Guid Id { get; set; }
    public Guid ResponseId { get; set; }
    public Guid EventOptionId { get; set; }

    public Response Response { get; set; } = null!;
    public EventOption EventOption { get; set; } = null!;
}
