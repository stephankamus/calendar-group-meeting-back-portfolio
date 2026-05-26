using CalendarGroupMeeting.Api.Application.DTOs;
using CalendarGroupMeeting.Api.Application.Services;

namespace CalendarGroupMeeting.Api.API;

public static class Endpoints
{
    public static void MapEventEndpoints(this WebApplication app)
    {
        app.MapGet("/health", GetHealth).WithOpenApi();
        app.MapPost("/events", CreateEvent).WithOpenApi();
        app.MapGet("/events/{token}", GetEvent).WithOpenApi();
        app.MapGet("/events/{token}/check-name", CheckName).WithOpenApi();
        app.MapGet("/events/{token}/results", GetEventResults).WithOpenApi();
        app.MapPut("/events/{token}", UpdateEvent).WithOpenApi();
        app.MapPut("/events/{token}/description", UpdateEventDescription).WithOpenApi();
        app.MapDelete("/events/{token}/options/{optionId}", DeleteEventOption).WithOpenApi();
        app.MapGet("/events/{token}/user-response", GetUserResponse).WithOpenApi();
        app.MapPost("/responses", CreateResponse).WithOpenApi();
        app.MapPut("/responses/{responseToken}", UpdateResponse).WithOpenApi();
    }

    private static async Task<IResult> GetHealth()
    {
        return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }

    private static async Task<IResult> CreateEvent(CreateEventRequest request, IEventService eventService)
    {
        var response = await eventService.CreateEventAsync(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetEvent(string token, IEventService eventService)
    {
        var response = await eventService.GetEventAsync(token);
        return response is not null ? Results.Ok(response) : Results.NotFound();
    }

    private static async Task<IResult> CheckName(string token, string name, IEventService eventService)
    {
        var response = await eventService.CheckNameExistsAsync(token, name);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetEventResults(string token, IEventService eventService)
    {
        var response = await eventService.GetEventResultsAsync(token);
        return response is not null ? Results.Ok(response) : Results.NotFound();
    }

    private static async Task<IResult> UpdateEvent(string token, UpdateEventRequest request, IEventService eventService)
    {
        var response = await eventService.UpdateEventAsync(token, request);
        return response.Success ? Results.Ok(response) : Results.NotFound();
    }
        
    private static async Task<IResult> UpdateEventDescription(string token, UpdateEventDescriptionInput request, IEventService eventService)
    {
        var response = await eventService.UpdateEventDescriptionAsync(token, request);
        return response.Success ? Results.Ok(response) : Results.NotFound();
    }

        
    private static async Task<IResult> DeleteEventOption(string token, Guid optionId, IEventService eventService)
    {
        var success = await eventService.DeleteEventOptionAsync(token, optionId);
        return success ? Results.NoContent() : Results.NotFound();
    }
        
    private static async Task<IResult> GetUserResponse(string token, string name, IEventService eventService)
    {
        var response = await eventService.GetUserResponseAsync(token, name);
        return Results.Ok(response);
    }

    private static async Task<IResult> CreateResponse(CreateResponseRequest request, IResponseService responseService)
    {
        var response = await responseService.CreateResponseAsync(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateResponse(string responseToken, UpdateResponseRequest request, IResponseService responseService)
    {
        var success = await responseService.UpdateResponseAsync(responseToken, request);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
