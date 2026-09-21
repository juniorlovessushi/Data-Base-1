using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/events/{eventId}/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public CategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(int eventId)
    {
        var categories = await _context.Categories
            .Where(c => c.EventId == eventId)
            .Select(c => new CategoryResponseDto(c.CategoryId, c.EventId, c.CategoryName, c.MaxParticipants))
            .ToListAsync();

        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateCategory(int eventId, [FromBody] CreateCategoryDto dto)
    {
        var raceEvent = await _context.Events.FindAsync(eventId);
        if (raceEvent == null) return NotFound("Event not found.");

        var category = new Category
        {
            EventId = eventId,
            CategoryName = dto.CategoryName,
            MaxParticipants = dto.MaxParticipants
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return StatusCode(201, category);
    }
}