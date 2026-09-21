using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;
using RaceDay.API.Services;
using System.Security.Claims;

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

    // GET: api/events
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _context.Events
            .Include(e => e.Organiser)
            .Select(e => new EventResponseDto(
                e.EventId,
                e.EventName,
                e.EventDate,
                e.Location,
                e.OrganiserId,
                e.Organiser.FullName,
                e.BannerImageUrl
            ))
            .ToListAsync();

        return Ok(events);
    }

    // POST: api/events
    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateEvent(
        [FromBody] CreateEventDto dto)
    {
        var organiserIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (organiserIdClaim == null)
        {
            return Unauthorized("Organiser ID was not found.");
        }

        var organiserId =
            int.Parse(organiserIdClaim.Value);

        var raceEvent = new Event
        {
            EventName = dto.EventName,
            EventDate = dto.EventDate,
            Location = dto.Location,
            OrganiserId = organiserId
        };

        _context.Events.Add(raceEvent);

        await _context.SaveChangesAsync();

        return StatusCode(201, new EventResponseDto(
            raceEvent.EventId,
            raceEvent.EventName,
            raceEvent.EventDate,
            raceEvent.Location,
            raceEvent.OrganiserId,
            "",
            raceEvent.BannerImageUrl
        ));
    }

    // PUT: api/events/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEvent(
        int id,
        [FromBody] CreateEventDto dto)
    {
        var organiserIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (organiserIdClaim == null)
        {
            return Unauthorized();
        }

        var organiserId =
            int.Parse(organiserIdClaim.Value);

        var raceEvent =
            await _context.Events.FindAsync(id);

        if (raceEvent == null)
        {
            return NotFound("Event not found.");
        }

        if (raceEvent.OrganiserId != organiserId)
        {
            return Forbid();
        }

        raceEvent.EventName = dto.EventName;
        raceEvent.EventDate = dto.EventDate;
        raceEvent.Location = dto.Location;

        await _context.SaveChangesAsync();

        return Ok("Event updated successfully.");
    }

    // DELETE: api/events/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var organiserIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier);

        if (organiserIdClaim == null)
        {
            return Unauthorized();
        }

        var organiserId =
            int.Parse(organiserIdClaim.Value);

        var raceEvent =
            await _context.Events.FindAsync(id);

        if (raceEvent == null)
        {
            return NotFound("Event not found.");
        }

        if (raceEvent.OrganiserId != organiserId)
        {
            return Forbid();
        }

        _context.Events.Remove(raceEvent);

        await _context.SaveChangesAsync();

        return Ok("Event deleted successfully.");
    }

    // POST: api/events/{id}/upload-banner
    [HttpPost("{id}/upload-banner")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UploadBanner(
        int id,
        IFormFile file,
        [FromServices] IBlobService blobService)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file provided.");
        }

        var raceEvent =
            await _context.Events.FindAsync(id);

        if (raceEvent == null)
        {
            return NotFound("Event not found.");
        }

        var imageUrl =
            await blobService.UploadFileAsync(
                file,
                "event-banners"
            );

        raceEvent.BannerImageUrl = imageUrl;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            ImageUrl = imageUrl
        });
    }
}