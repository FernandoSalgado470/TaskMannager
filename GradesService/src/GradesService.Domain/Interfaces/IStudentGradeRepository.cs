using GradesService.Domain.Entities;

namespace GradesService.Domain.Interfaces;

public interface IStudentGradeRepository : IRepository<StudentGrade>
{
    Task<IEnumerable<StudentGrade>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<StudentGrade>> GetBySubjectIdAsync(int subjectId);
    Task<StudentGrade?> GetByStudentSubjectAndPeriodAsync(int studentId, int subjectId, int periodId);
    Task<IEnumerable<StudentGrade>> GetByAcademicPeriodAsync(int periodId);
}
