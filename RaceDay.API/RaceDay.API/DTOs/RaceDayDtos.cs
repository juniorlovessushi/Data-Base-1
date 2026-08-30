namespace RaceDay.API.DTOs;

// Authentication DTOs
public record RegisterDto(string FullName, string Email, string Password, int RoleId);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string FullName, string Role);

// Profile DTOs
public record UserProfileDto(int UserId, string FullName, string Email, string Role);
public record UpdateProfileDto(string FullName, string Email);

// Event DTOs
public record CreateEventDto(string EventName, DateTime EventDate, string Location);
public record EventResponseDto(int EventId, string EventName, DateTime EventDate, string Location, int OrganiserId, string OrganiserName);

// Category DTOs
public record CreateCategoryDto(string CategoryName, int MaxParticipants);
public record CategoryResponseDto(int CategoryId, int EventId, string CategoryName, int MaxParticipants);

// Enrolment DTOs
public record CreateEnrolmentDto(int CategoryId);
public record EnrolmentResponseDto(int EnrolmentId, int CategoryId, string CategoryName, int EventId, string EventName, int ParticipantId, string ParticipantName, DateTime EnrolmentDate);

// Result DTOs
public record CreateResultDto(int EnrolmentId, TimeSpan FinishTime, int Position);
public record ResultResponseDto(int ResultId, int EnrolmentId, string ParticipantName, string EventName, string CategoryName, TimeSpan FinishTime, int Position);