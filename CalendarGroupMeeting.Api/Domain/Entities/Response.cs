namespace CalendarGroupMeeting.Api.Domain.Entities;

public class Response
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Event Event { get; set; } = null!;
    public ICollection<ResponseOption> ResponseOptions { get; set; } = new List<ResponseOption>();
}
