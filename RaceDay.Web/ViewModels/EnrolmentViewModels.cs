namespace RaceDay.Web.ViewModels;

public class CreateEnrolmentViewModel
{
    public int CategoryId { get; set; }
    public string ParticipantName { get; set; } = string.Empty;
    public string ParticipantEmail { get; set; } = string.Empty;
}

public class EnrolmentViewModel
{
    public int EnrolmentId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public DateTime RaceDate { get; set; }
    public decimal FeePaid { get; set; }
}