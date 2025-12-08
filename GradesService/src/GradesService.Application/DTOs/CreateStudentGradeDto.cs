using System.ComponentModel.DataAnnotations;

namespace GradesService.Application.DTOs;

public class CreateStudentGradeDto
{
    [Required(ErrorMessage = "El ID del estudiante es requerido")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "El ID de la materia es requerido")]
    public int SubjectId { get; set; }

    [Required(ErrorMessage = "El ID del período académico es requerido")]
    public int AcademicPeriodId { get; set; }

    [Range(0, 100, ErrorMessage = "La calificación final debe estar entre 0 y 100")]
    public decimal FinalGrade { get; set; }

    [MaxLength(1000, ErrorMessage = "Los comentarios no pueden exceder 1000 caracteres")]
    public string? Comments { get; set; }

    public bool IsPassed { get; set; } = false;
}
