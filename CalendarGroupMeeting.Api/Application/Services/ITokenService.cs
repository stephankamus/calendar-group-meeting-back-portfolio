namespace CalendarGroupMeeting.Api.Application.Services;

public interface ITokenService
{
    string GenerateEventToken();
    string GenerateResponseToken();
}