namespace CalendarGroupMeeting.Api.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<EventOption> Options { get; set; } = new List<EventOption>();
    public ICollection<Response> Responses { get; set; } = new List<Response>();
}
