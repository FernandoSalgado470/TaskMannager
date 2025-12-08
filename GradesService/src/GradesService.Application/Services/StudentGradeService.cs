using GradesService.Application.DTOs;
using GradesService.Application.Interfaces;
using GradesService.Domain.Entities;
using GradesService.Domain.Interfaces;

namespace GradesService.Application.Services;

public class StudentGradeService : IStudentGradeService
{
    private readonly IStudentGradeRepository _studentGradeRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly IGradeCategoryRepository _gradeCategoryRepository;

    public StudentGradeService(
        IStudentGradeRepository studentGradeRepository,
        IGradeRepository gradeRepository,
        IGradeCategoryRepository gradeCategoryRepository)
    {
        _studentGradeRepository = studentGradeRepository;
        _gradeRepository = gradeRepository;
        _gradeCategoryRepository = gradeCategoryRepository;
    }

    public async Task<IEnumerable<StudentGradeDto>> GetAllStudentGradesAsync()
    {
        var studentGrades = await _studentGradeRepository.GetAllAsync();
        return studentGrades.Select(MapToDto);
    }

    public async Task<StudentGradeDto?> GetStudentGradeByIdAsync(int id)
    {
        var studentGrade = await _studentGradeRepository.GetByIdAsync(id);
        return studentGrade != null ? MapToDto(studentGrade) : null;
    }

    public async Task<IEnumerable<StudentGradeDto>> GetStudentGradesByStudentIdAsync(int studentId)
    {
        var studentGrades = await _studentGradeRepository.GetByStudentIdAsync(studentId);
        return studentGrades.Select(MapToDto);
    }

    public async Task<IEnumerable<StudentGradeDto>> GetStudentGradesBySubjectIdAsync(int subjectId)
    {
        var studentGrades = await _studentGradeRepository.GetBySubjectIdAsync(subjectId);
        return studentGrades.Select(MapToDto);
    }

    public async Task<StudentGradeDto> CreateStudentGradeAsync(CreateStudentGradeDto createStudentGradeDto)
    {
        // Validar que no exista ya una calificación final para este estudiante, materia y período
        var existing = await _studentGradeRepository.GetByStudentSubjectAndPeriodAsync(
            createStudentGradeDto.StudentId,
            createStudentGradeDto.SubjectId,
            createStudentGradeDto.AcademicPeriodId);

        if (existing != null)
        {
            throw new InvalidOperationException("Ya existe una calificación final para este estudiante, materia y período");
        }

        var studentGrade = new StudentGrade
        {
            StudentId = createStudentGradeDto.StudentId,
            SubjectId = createStudentGradeDto.SubjectId,
            AcademicPeriodId = createStudentGradeDto.AcademicPeriodId,
            FinalGrade = createStudentGradeDto.FinalGrade,
            Comments = createStudentGradeDto.Comments,
            IsPassed = createStudentGradeDto.IsPassed,
            CreatedAt = DateTime.UtcNow
        };

        var createdStudentGrade = await _studentGradeRepository.CreateAsync(studentGrade);
        return MapToDto(createdStudentGrade);
    }

    public async Task<StudentGradeDto> UpdateStudentGradeAsync(int id, CreateStudentGradeDto updateStudentGradeDto)
    {
        var studentGrade = await _studentGradeRepository.GetByIdAsync(id);
        if (studentGrade == null)
        {
            throw new InvalidOperationException("Calificación de estudiante no encontrada");
        }

        studentGrade.StudentId = updateStudentGradeDto.StudentId;
        studentGrade.SubjectId = updateStudentGradeDto.SubjectId;
        studentGrade.AcademicPeriodId = updateStudentGradeDto.AcademicPeriodId;
        studentGrade.FinalGrade = updateStudentGradeDto.FinalGrade;
        studentGrade.Comments = updateStudentGradeDto.Comments;
        studentGrade.IsPassed = updateStudentGradeDto.IsPassed;
        studentGrade.UpdatedAt = DateTime.UtcNow;

        var updatedStudentGrade = await _studentGradeRepository.UpdateAsync(studentGrade);
        return MapToDto(updatedStudentGrade);
    }

    public async Task<bool> DeleteStudentGradeAsync(int id)
    {
        return await _studentGradeRepository.DeleteAsync(id);
    }

    public async Task<StudentGradeDto> CalculateFinalGradeAsync(int studentId, int subjectId, int periodId)
    {
        // Obtener todas las calificaciones del estudiante para la materia y período
        var grades = await _gradeRepository.GetByStudentSubjectAndPeriodAsync(studentId, subjectId, periodId);

        if (!grades.Any())
        {
            throw new InvalidOperationException("No hay calificaciones registradas para calcular la nota final");
        }

        // Obtener todas las categorías activas
        var categories = await _gradeCategoryRepository.GetActiveAsync();
        var categoriesList = categories.ToList();

        decimal finalGrade = 0;

        if (categoriesList.Any())
        {
            // Calcular promedio ponderado por categoría
            foreach (var category in categoriesList)
            {
                var categoryGrades = grades.Where(g => g.GradeCategoryId == category.Id).ToList();

                if (categoryGrades.Any())
                {
                    // Calcular promedio de la categoría
                    var categoryAverage = categoryGrades.Average(g => g.PercentageScore);

                    // Aplicar peso de la categoría
                    finalGrade += categoryAverage * (category.WeightPercentage / 100);
                }
            }
        }
        else
        {
            // Si no hay categorías, calcular promedio simple
            finalGrade = grades.Average(g => g.PercentageScore);
        }

        // Determinar si aprobó (>=70)
        bool isPassed = finalGrade >= 70;

        // Buscar si ya existe una calificación final
        var existingGrade = await _studentGradeRepository.GetByStudentSubjectAndPeriodAsync(studentId, subjectId, periodId);

        if (existingGrade != null)
        {
            // Actualizar existente
            existingGrade.FinalGrade = Math.Round(finalGrade, 2);
            existingGrade.IsPassed = isPassed;
            existingGrade.UpdatedAt = DateTime.UtcNow;
            existingGrade.Comments = $"Calculado automáticamente el {DateTime.UtcNow:dd/MM/yyyy}";

            var updated = await _studentGradeRepository.UpdateAsync(existingGrade);
            return MapToDto(updated);
        }
        else
        {
            // Crear nueva
            var newGrade = new StudentGrade
            {
                StudentId = studentId,
                SubjectId = subjectId,
                AcademicPeriodId = periodId,
                FinalGrade = Math.Round(finalGrade, 2),
                IsPassed = isPassed,
                Comments = $"Calculado automáticamente el {DateTime.UtcNow:dd/MM/yyyy}",
                CreatedAt = DateTime.UtcNow
            };

            var created = await _studentGradeRepository.CreateAsync(newGrade);
            return MapToDto(created);
        }
    }

    private StudentGradeDto MapToDto(StudentGrade studentGrade)
    {
        return new StudentGradeDto
        {
            Id = studentGrade.Id,
            StudentId = studentGrade.StudentId,
            SubjectId = studentGrade.SubjectId,
            AcademicPeriodId = studentGrade.AcademicPeriodId,
            FinalGrade = studentGrade.FinalGrade,
            Comments = studentGrade.Comments,
            IsPassed = studentGrade.IsPassed,
            CreatedAt = studentGrade.CreatedAt,
            UpdatedAt = studentGrade.UpdatedAt
        };
    }
}
