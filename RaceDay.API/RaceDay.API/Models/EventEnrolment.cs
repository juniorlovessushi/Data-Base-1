namespace RaceDay.API.Models;

public class EventEnrolment
{
    public int EnrolmentId { get; set; }
    public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int ParticipantId { get; set; }
    public User Participant { get; set; } = null!;

    public Result? Result { get; set; }
}