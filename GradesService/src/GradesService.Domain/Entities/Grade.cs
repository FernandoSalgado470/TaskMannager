namespace GradesService.Domain.Entities;

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; } // Foreign Key to UserService
    public int SubjectId { get; set; } // Foreign Key to AcademicService
    public int AcademicPeriodId { get; set; } // Foreign Key to AcademicService
    public int? GradeCategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Score { get; set; } // Calificación obtenida
    public decimal MaxScore { get; set; } // Calificación máxima posible
    public DateTime GradeDate { get; set; }
    public string? Comments { get; set; }
    public int? TaskId { get; set; } // Foreign Key to TaskService (opcional)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public GradeCategory? GradeCategory { get; set; }

    // Calculated property
    public decimal PercentageScore => MaxScore > 0 ? (Score / MaxScore) * 100 : 0;
}
