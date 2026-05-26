using CalendarGroupMeeting.Api.Application.Services;

namespace CalendarGroupMeeting.Api.Infra.DependencyInjection;

public static class Application
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IResponseService, ResponseService>();
    }
}
