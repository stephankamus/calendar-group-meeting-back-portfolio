using CalendarGroupMeeting.Api.Application.DTOs;
using CalendarGroupMeeting.Api.Domain.Entities;
using CalendarGroupMeeting.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarGroupMeeting.Api.Application.Services;

public class ResponseService : IResponseService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public ResponseService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<CreateResponseResponse> CreateResponseAsync(CreateResponseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required");

        if (request.SelectedOptionIds == null || !request.SelectedOptionIds.Any())
            throw new ArgumentException("At least one option must be selected");

        var eventEntity = await _context.Events
            .Include(e => e.Options)
            .FirstOrDefaultAsync(e => e.Token == request.EventToken);

        if (eventEntity == null)
            throw new ArgumentException("Event not found");

        var nameExists = await _context.Responses
            .AnyAsync(r => r.EventId == eventEntity.Id && r.Name == request.Name);

        if (nameExists)
            throw new ArgumentException("Name already exists for this event");

        var validOptionIds = eventEntity.Options.Select(o => o.Id).ToHashSet();
        if (!request.SelectedOptionIds.All(id => validOptionIds.Contains(id)))
            throw new ArgumentException("Invalid option IDs");

        var responseToken = _tokenService.GenerateResponseToken();

        while (await _context.Responses.AnyAsync(r => r.Token == responseToken))
        {
            responseToken = _tokenService.GenerateResponseToken();
        }

        var response = new Response
        {
            Id = Guid.NewGuid(),
            EventId = eventEntity.Id,
            Name = request.Name,
            Token = responseToken,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        foreach (var optionId in request.SelectedOptionIds.Distinct())
        {
            response.ResponseOptions.Add(new ResponseOption
            {
                Id = Guid.NewGuid(),
                EventOptionId = optionId
            });
        }

        _context.Responses.Add(response);
        await _context.SaveChangesAsync();

        return new CreateResponseResponse(responseToken);
    }

    public async Task<bool> UpdateResponseAsync(string responseToken, UpdateResponseRequest request)
    {
        if (request.SelectedOptionIds == null || !request.SelectedOptionIds.Any())
            throw new ArgumentException("At least one option must be selected");

        var response = await _context.Responses
            .Include(r => r.ResponseOptions)
            .Include(r => r.Event)
            .ThenInclude(e => e.Options)
            .FirstOrDefaultAsync(r => r.Token == responseToken);

        if (response == null)
            return false;

        var validOptionIds = response.Event.Options.Select(o => o.Id).ToHashSet();
        if (!request.SelectedOptionIds.All(id => validOptionIds.Contains(id)))
            throw new ArgumentException("Invalid option IDs");

        _context.ResponseOptions.RemoveRange(response.ResponseOptions);

        var newResponseOptions = request.SelectedOptionIds.Distinct()
            .Select(optionId => new ResponseOption
            {
                Id = Guid.NewGuid(),
                ResponseId = response.Id,
                EventOptionId = optionId
            })
            .ToList();

        await _context.ResponseOptions.AddRangeAsync(newResponseOptions);

        response.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }
}
