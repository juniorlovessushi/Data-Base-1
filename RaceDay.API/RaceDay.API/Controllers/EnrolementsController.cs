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
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Enrol([FromBody] CreateEnrolmentDto dto)
    {
        var participantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var category = await _context.Categories.Include(c => c.Enrolments).FirstOrDefaultAsync(c => c.CategoryId == dto.CategoryId);
        if (category == null) return NotFound("Category not found.");

        if (category.Enrolments.Count >= category.MaxParticipants)
            return BadRequest("Category has reached maximum participant limit.");

        var exists = await _context.EventEnrolments.AnyAsync(e => e.CategoryId == dto.CategoryId && e.ParticipantId == participantId);
        if (exists) return Conflict("You are already enrolled in this category.");

        var enrolment = new EventEnrolment
        {
            CategoryId = dto.CategoryId,
            ParticipantId = participantId,
            EnrolmentDate = DateTime.UtcNow
        };

        _context.EventEnrolments.Add(enrolment);
        await _context.SaveChangesAsync();

        return StatusCode(201, "Enrolment successful.");
    }

    [HttpGet("my-races")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> GetMyRaces()
    {
        var participantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var enrolments = await _context.EventEnrolments
            .Include(e => e.Category).ThenInclude(c => c.Event)
            .Include(e => e.Participant)
            .Where(e => e.ParticipantId == participantId)
            .Select(e => new EnrolmentResponseDto(
                e.EnrolmentId,
                e.CategoryId,
                e.Category.CategoryName,
                e.Category.EventId,
                e.Category.Event.EventName,
                e.ParticipantId,
                e.Participant.FullName,
                e.EnrolmentDate))
            .ToListAsync();

        return Ok(enrolments);
    }
}