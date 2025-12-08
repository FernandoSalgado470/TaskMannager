using GradesService.Application.DTOs;

namespace GradesService.Application.Interfaces;

public interface IGradeService
{
    Task<IEnumerable<GradeDto>> GetAllGradesAsync();
    Task<GradeDto?> GetGradeByIdAsync(int id);
    Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(int studentId);
    Task<IEnumerable<GradeDto>> GetGradesBySubjectIdAsync(int subjectId);
    Task<IEnumerable<GradeDto>> GetGradesByStudentAndSubjectAsync(int studentId, int subjectId);
    Task<GradeDto> CreateGradeAsync(CreateGradeDto createGradeDto);
    Task<GradeDto> UpdateGradeAsync(int id, CreateGradeDto updateGradeDto);
    Task<bool> DeleteGradeAsync(int id);
}
