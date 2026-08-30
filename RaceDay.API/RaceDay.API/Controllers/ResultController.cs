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
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> RecordResult([FromBody] CreateResultDto dto)
    {
        var enrolment = await _context.EventEnrolments.FindAsync(dto.EnrolmentId);
        if (enrolment == null) return NotFound("Enrolment not found.");

        var existingResult = await _context.Results.FirstOrDefaultAsync(r => r.EnrolmentId == dto.EnrolmentId);
        if (existingResult != null) return Conflict("Result already recorded for this enrolment.");

        var result = new Result
        {
            EnrolmentId = dto.EnrolmentId,
            FinishTime = dto.FinishTime,
            Position = dto.Position
        };

        _context.Results.Add(result);
        await _context.SaveChangesAsync();

        return StatusCode(201, result);
    }

    [HttpGet("my-results")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> GetMyResults()
    {
        var participantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var results = await _context.Results
            .Include(r => r.Enrolment).ThenInclude(e => e.Participant)
            .Include(r => r.Enrolment).ThenInclude(e => e.Category).ThenInclude(c => c.Event)
            .Where(r => r.Enrolment.ParticipantId == participantId)
            .Select(r => new ResultResponseDto(
                r.ResultId,
                r.EnrolmentId,
                r.Enrolment.Participant.FullName,
                r.Enrolment.Category.Event.EventName,
                r.Enrolment.Category.CategoryName,
                r.FinishTime,
                r.Position))
            .ToListAsync();

        return Ok(results);
    }
}