using GradesService.Domain.Entities;

namespace GradesService.Domain.Interfaces;

public interface IGradeRepository : IRepository<Grade>
{
    Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Grade>> GetBySubjectIdAsync(int subjectId);
    Task<IEnumerable<Grade>> GetByStudentAndSubjectAsync(int studentId, int subjectId);
    Task<IEnumerable<Grade>> GetByStudentSubjectAndPeriodAsync(int studentId, int subjectId, int periodId);
    Task<IEnumerable<Grade>> GetByCategoryIdAsync(int categoryId);
}
