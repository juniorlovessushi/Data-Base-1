using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _context.Events.Include(e => e.Organiser)
            .Select(e => new EventResponseDto(e.EventId, e.EventName, e.EventDate, e.Location, e.OrganiserId, e.Organiser.FullName))
            .ToListAsync();

        return Ok(events);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
    {
        var organiserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var raceEvent = new Event
        {
            EventName = dto.EventName,
            EventDate = dto.EventDate,
            Location = dto.Location,
            OrganiserId = organiserId
        };

        _context.Events.Add(raceEvent);
        await _context.SaveChangesAsync();

        return StatusCode(201, raceEvent);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] CreateEventDto dto)
    {
        var organiserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var raceEvent = await _context.Events.FindAsync(id);

        if (raceEvent == null) return NotFound("Event not found.");
        if (raceEvent.OrganiserId != organiserId) return Forbid();

        raceEvent.EventName = dto.EventName;
        raceEvent.EventDate = dto.EventDate;
        raceEvent.Location = dto.Location;

        await _context.SaveChangesAsync();
        return Ok("Event updated successfully.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var organiserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var raceEvent = await _context.Events.FindAsync(id);

        if (raceEvent == null) return NotFound("Event not found.");
        if (raceEvent.OrganiserId != organiserId) return Forbid();

        _context.Events.Remove(raceEvent);
        await _context.SaveChangesAsync();
        return Ok("Event deleted successfully.");
    }
}