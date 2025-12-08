using GradesService.Application.DTOs;
using GradesService.Application.Interfaces;
using GradesService.Domain.Entities;
using GradesService.Domain.Interfaces;

namespace GradesService.Application.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IGradeCategoryRepository _gradeCategoryRepository;

    public GradeService(IGradeRepository gradeRepository, IGradeCategoryRepository gradeCategoryRepository)
    {
        _gradeRepository = gradeRepository;
        _gradeCategoryRepository = gradeCategoryRepository;
    }

    public async Task<IEnumerable<GradeDto>> GetAllGradesAsync()
    {
        var grades = await _gradeRepository.GetAllAsync();
        return grades.Select(MapToDto);
    }

    public async Task<GradeDto?> GetGradeByIdAsync(int id)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);
        return grade != null ? MapToDto(grade) : null;
    }

    public async Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(int studentId)
    {
        var grades = await _gradeRepository.GetByStudentIdAsync(studentId);
        return grades.Select(MapToDto);
    }

    public async Task<IEnumerable<GradeDto>> GetGradesBySubjectIdAsync(int subjectId)
    {
        var grades = await _gradeRepository.GetBySubjectIdAsync(subjectId);
        return grades.Select(MapToDto);
    }

    public async Task<IEnumerable<GradeDto>> GetGradesByStudentAndSubjectAsync(int studentId, int subjectId)
    {
        var grades = await _gradeRepository.GetByStudentAndSubjectAsync(studentId, subjectId);
        return grades.Select(MapToDto);
    }

    public async Task<GradeDto> CreateGradeAsync(CreateGradeDto createGradeDto)
    {
        // Validar que la calificación no exceda el máximo
        if (createGradeDto.Score > createGradeDto.MaxScore)
        {
            throw new InvalidOperationException("La calificación no puede ser mayor a la calificación máxima");
        }

        var grade = new Grade
        {
            StudentId = createGradeDto.StudentId,
            SubjectId = createGradeDto.SubjectId,
            AcademicPeriodId = createGradeDto.AcademicPeriodId,
            GradeCategoryId = createGradeDto.GradeCategoryId,
            Title = createGradeDto.Title,
            Description = createGradeDto.Description,
            Score = createGradeDto.Score,
            MaxScore = createGradeDto.MaxScore,
            GradeDate = createGradeDto.GradeDate,
            Comments = createGradeDto.Comments,
            TaskId = createGradeDto.TaskId,
            CreatedAt = DateTime.UtcNow
        };

        var createdGrade = await _gradeRepository.CreateAsync(grade);
        return MapToDto(createdGrade);
    }

    public async Task<GradeDto> UpdateGradeAsync(int id, CreateGradeDto updateGradeDto)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);
        if (grade == null)
        {
            throw new InvalidOperationException("Calificación no encontrada");
        }

        // Validar que la calificación no exceda el máximo
        if (updateGradeDto.Score > updateGradeDto.MaxScore)
        {
            throw new InvalidOperationException("La calificación no puede ser mayor a la calificación máxima");
        }

        grade.StudentId = updateGradeDto.StudentId;
        grade.SubjectId = updateGradeDto.SubjectId;
        grade.AcademicPeriodId = updateGradeDto.AcademicPeriodId;
        grade.GradeCategoryId = updateGradeDto.GradeCategoryId;
        grade.Title = updateGradeDto.Title;
        grade.Description = updateGradeDto.Description;
        grade.Score = updateGradeDto.Score;
        grade.MaxScore = updateGradeDto.MaxScore;
        grade.GradeDate = updateGradeDto.GradeDate;
        grade.Comments = updateGradeDto.Comments;
        grade.TaskId = updateGradeDto.TaskId;
        grade.UpdatedAt = DateTime.UtcNow;

        var updatedGrade = await _gradeRepository.UpdateAsync(grade);
        return MapToDto(updatedGrade);
    }

    public async Task<bool> DeleteGradeAsync(int id)
    {
        return await _gradeRepository.DeleteAsync(id);
    }

    private GradeDto MapToDto(Grade grade)
    {
        return new GradeDto
        {
            Id = grade.Id,
            StudentId = grade.StudentId,
            SubjectId = grade.SubjectId,
            AcademicPeriodId = grade.AcademicPeriodId,
            GradeCategoryId = grade.GradeCategoryId,
            Title = grade.Title,
            Description = grade.Description,
            Score = grade.Score,
            MaxScore = grade.MaxScore,
            GradeDate = grade.GradeDate,
            Comments = grade.Comments,
            TaskId = grade.TaskId,
            CreatedAt = grade.CreatedAt,
            UpdatedAt = grade.UpdatedAt,
            PercentageScore = grade.PercentageScore,
            GradeCategory = grade.GradeCategory != null ? MapCategoryToDto(grade.GradeCategory) : null
        };
    }

    private GradeCategoryDto MapCategoryToDto(GradeCategory category)
    {
        return new GradeCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            WeightPercentage = category.WeightPercentage,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt
        };
    }
}
