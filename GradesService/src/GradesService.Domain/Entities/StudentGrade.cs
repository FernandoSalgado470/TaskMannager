namespace GradesService.Domain.Entities;

public class StudentGrade
{
    public int Id { get; set; }
    public int StudentId { get; set; } // Foreign Key to UserService
    public int SubjectId { get; set; } // Foreign Key to AcademicService
    public int AcademicPeriodId { get; set; } // Foreign Key to AcademicService
    public decimal FinalGrade { get; set; }
    public string? Comments { get; set; }
    public bool IsPassed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
