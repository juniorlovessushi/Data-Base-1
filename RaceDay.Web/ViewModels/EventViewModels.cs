using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RaceDay.Web.ViewModels;

public class EventViewModel
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string OrganiserName { get; set; } = string.Empty;
    public string? BannerImageUrl { get; set; }
}

public class CreateEventViewModel
{
    [Required]
    public string EventName { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; } = DateTime.Now.AddDays(7);

    [Required]
    public string Location { get; set; } = string.Empty;

    public IFormFile? BannerImage { get; set; }
}