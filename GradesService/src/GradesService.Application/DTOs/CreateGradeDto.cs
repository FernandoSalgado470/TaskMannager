using System.ComponentModel.DataAnnotations;

namespace GradesService.Application.DTOs;

public class CreateGradeDto
{
    [Required(ErrorMessage = "El ID del estudiante es requerido")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "El ID de la materia es requerido")]
    public int SubjectId { get; set; }

    [Required(ErrorMessage = "El ID del período académico es requerido")]
    public int AcademicPeriodId { get; set; }

    public int? GradeCategoryId { get; set; }

    [Required(ErrorMessage = "El título es requerido")]
    [MaxLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "La calificación debe ser mayor o igual a 0")]
    public decimal Score { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "La calificación máxima debe ser mayor a 0")]
    public decimal MaxScore { get; set; } = 100;

    public DateTime GradeDate { get; set; } = DateTime.UtcNow;

    [MaxLength(1000, ErrorMessage = "Los comentarios no pueden exceder 1000 caracteres")]
    public string? Comments { get; set; }

    public int? TaskId { get; set; }
}
