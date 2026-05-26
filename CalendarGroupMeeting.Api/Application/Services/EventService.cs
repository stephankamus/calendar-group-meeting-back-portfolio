using CalendarGroupMeeting.Api.Application.DTOs;
using CalendarGroupMeeting.Api.Domain.Entities;
using CalendarGroupMeeting.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarGroupMeeting.Api.Application.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public EventService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<CreateEventResponse> CreateEventAsync(CreateEventRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Event name is required");

        if (request.Options == null || !request.Options.Any())
            throw new ArgumentException("At least one option is required");

        var eventToken = _tokenService.GenerateEventToken();

        while (await _context.Events.AnyAsync(e => e.Token == eventToken))
        {
            eventToken = _tokenService.GenerateEventToken();
        }

        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Token = eventToken,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var option in request.Options)
        {
            if (option.StartDateTime >= option.EndDateTime)
                throw new ArgumentException("Start time must be before end time");

            eventEntity.Options.Add(new EventOption
            {
                Id = Guid.NewGuid(),
                StartDateTime = option.StartDateTime.ToUniversalTime(),
                EndDateTime = option.EndDateTime.ToUniversalTime()
            });
        }

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();

        return new CreateEventResponse(eventToken);
    }

    public async Task<GetEventResponse?> GetEventAsync(string token)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Options)
            .FirstOrDefaultAsync(e => e.Token == token);

        if (eventEntity == null)
            return null;

        return new GetEventResponse(
            eventEntity.Name,
            eventEntity.Description,
            eventEntity.Options.Select(o => new EventOptionResponse(
                o.Id,
                o.StartDateTime,
                o.EndDateTime
            )).ToList()
        );
    }

    public async Task<CheckNameResponse> CheckNameExistsAsync(string eventToken, string name)
    {
        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(e => e.Token == eventToken);

        if (eventEntity == null)
            return new CheckNameResponse(false);

        var exists = await _context.Responses
            .AnyAsync(r => r.EventId == eventEntity.Id && r.Name == name);

        return new CheckNameResponse(exists);
    }

    public async Task<EventResultResponse?> GetEventResultsAsync(string token)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Options)
            .ThenInclude(o => o.ResponseOptions)
            .ThenInclude(r => r.Response)
            .FirstOrDefaultAsync(e => e.Token == token);

        if (eventEntity == null)
            return null;

        var results = eventEntity.Options.Select(option => new OptionResultDto(
            option.Id,
            option.ResponseOptions.Count,
            option.ResponseOptions.Select(n => n.Response.Name).ToList()
        )).ToList();

        return new EventResultResponse(results);
    }

    public async Task<UpdateEventResponse> UpdateEventAsync(string token, UpdateEventRequest request)
    {
        if (request.NewOptions == null || !request.NewOptions.Any())
            throw new ArgumentException("At least one new option is required");

        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(e => e.Token == token);

        if (eventEntity == null)
            return new UpdateEventResponse(false);

        foreach (var option in request.NewOptions)
        {
            if (option.StartDateTime >= option.EndDateTime)
                throw new ArgumentException("Start time must be before end time");

            var newOption = new EventOption
            {
                Id = Guid.NewGuid(),
                EventId = eventEntity.Id,
                StartDateTime = option.StartDateTime.ToUniversalTime(),
                EndDateTime = option.EndDateTime.ToUniversalTime()
            };

            _context.Set<EventOption>().Add(newOption);
        }

        await _context.SaveChangesAsync();

        return new UpdateEventResponse(true);
    }

    public async Task<bool> DeleteEventOptionAsync(string eventToken, Guid optionId)
    {
        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(e => e.Token == eventToken);

        if (eventEntity == null)
            return false;

        var eventOption = await _context.Set<EventOption>()
            .Include(r => r.ResponseOptions)
            .FirstOrDefaultAsync(eo => eo.Id == optionId && eo.EventId == eventEntity.Id);

        if (eventOption == null)
            return false;

        if (eventOption.ResponseOptions.Count > 0)
            return false;

        _context.Set<EventOption>().Remove(eventOption);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<GetUserResponseResponse> GetUserResponseAsync(string eventToken, string name)
    {
        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(e => e.Token == eventToken);

        if (eventEntity == null)
            throw new ArgumentException("Event not found");

        var response = await _context.Responses
            .Include(r => r.ResponseOptions)
            .FirstOrDefaultAsync(r => r.EventId == eventEntity.Id && r.Name == name);

        if (response == null)
        {
            return new GetUserResponseResponse(
                Exists: false,
                ResponseToken: null,
                SelectedOptionIds: new List<Guid>()
            );
        }

        var selectedOptionIds = response.ResponseOptions
            .Select(ro => ro.EventOptionId)
            .ToList();

        return new GetUserResponseResponse(
            Exists: true,
            ResponseToken: response.Token,
            SelectedOptionIds: selectedOptionIds
        );
    }

    public async Task<UpdateEventResponse> UpdateEventDescriptionAsync(string token, UpdateEventDescriptionInput request)
    {
        if (request.Description?.Length > 500)
            throw new ArgumentException("Descrição deve ter no máximo 500 caracteres");

        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(e => e.Token == token);

        if (eventEntity == null)
            return new UpdateEventResponse(false);

        eventEntity.Description = request.Description;

        await _context.SaveChangesAsync();

        return new UpdateEventResponse(true);
    }
}
