namespace LoginService.Domain.Entities;

public class LoginAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsSuccessful { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
    public string? FailureReason { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
}
