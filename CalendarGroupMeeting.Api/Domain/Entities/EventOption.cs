namespace CalendarGroupMeeting.Api.Domain.Entities;

public class EventOption
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public Event Event { get; set; } = null!;
    public ICollection<ResponseOption> ResponseOptions { get; set; } = new List<ResponseOption>();
}
