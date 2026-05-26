using CalendarGroupMeeting.Api.Application.DTOs;

namespace CalendarGroupMeeting.Api.Application.Services;

public interface IResponseService
{
    Task<CreateResponseResponse> CreateResponseAsync(CreateResponseRequest request);
    Task<bool> UpdateResponseAsync(string responseToken, UpdateResponseRequest request);
}
