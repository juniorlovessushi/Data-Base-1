namespace RaceDay.API.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
}