namespace GradesService.Domain.Entities;

public class GradeCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal WeightPercentage { get; set; } // Peso porcentual (ej: 30% para exámenes)
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
