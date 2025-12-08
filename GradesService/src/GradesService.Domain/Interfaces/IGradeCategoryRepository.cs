using GradesService.Domain.Entities;

namespace GradesService.Domain.Interfaces;

public interface IGradeCategoryRepository : IRepository<GradeCategory>
{
    Task<IEnumerable<GradeCategory>> GetActiveAsync();
    Task<GradeCategory?> GetByNameAsync(string name);
}
