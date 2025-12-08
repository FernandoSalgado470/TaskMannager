using System.ComponentModel.DataAnnotations;

namespace GradesService.Application.DTOs;

public class GradeCategoryDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "El peso porcentual debe estar entre 0 y 100")]
    public decimal WeightPercentage { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
