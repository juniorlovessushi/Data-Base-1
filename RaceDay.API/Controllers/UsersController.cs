using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public UsersController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("User not found.");

        return Ok(new
        {
            user.UserId,
            user.FullName,
            user.Email,
            user.ProfilePictureUrl
        });
    }

    [HttpPost("upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file, [FromServices] IBlobService blobService)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound("User not found.");

        var imageUrl = await blobService.UploadFileAsync(file, "profile-pictures");
        user.ProfilePictureUrl = imageUrl;
        await _context.SaveChangesAsync();

        return Ok(new { ImageUrl = imageUrl });
    }
}