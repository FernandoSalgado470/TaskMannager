using GradesService.Application.DTOs;

namespace GradesService.Application.Interfaces;

public interface IStudentGradeService
{
    Task<IEnumerable<StudentGradeDto>> GetAllStudentGradesAsync();
    Task<StudentGradeDto?> GetStudentGradeByIdAsync(int id);
    Task<IEnumerable<StudentGradeDto>> GetStudentGradesByStudentIdAsync(int studentId);
    Task<IEnumerable<StudentGradeDto>> GetStudentGradesBySubjectIdAsync(int subjectId);
    Task<StudentGradeDto> CreateStudentGradeAsync(CreateStudentGradeDto createStudentGradeDto);
    Task<StudentGradeDto> UpdateStudentGradeAsync(int id, CreateStudentGradeDto updateStudentGradeDto);
    Task<bool> DeleteStudentGradeAsync(int id);
    Task<StudentGradeDto> CalculateFinalGradeAsync(int studentId, int subjectId, int periodId);
}
