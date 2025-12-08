using GradesService.Domain.Entities;
using GradesService.Domain.Interfaces;
using GradesService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GradesService.Infrastructure.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly GradesDbContext _context;

    public GradeRepository(GradesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .ToListAsync();
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId)
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .Where(g => g.StudentId == studentId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetBySubjectIdAsync(int subjectId)
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .Where(g => g.SubjectId == subjectId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByStudentAndSubjectAsync(int studentId, int subjectId)
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .Where(g => g.StudentId == studentId && g.SubjectId == subjectId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByStudentSubjectAndPeriodAsync(int studentId, int subjectId, int periodId)
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .Where(g => g.StudentId == studentId && g.SubjectId == subjectId && g.AcademicPeriodId == periodId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Grades
            .Include(g => g.GradeCategory)
            .Where(g => g.GradeCategoryId == categoryId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<Grade> CreateAsync(Grade entity)
    {
        _context.Grades.Add(entity);
        await _context.SaveChangesAsync();

        // Recargar con la categoría incluida
        await _context.Entry(entity).Reference(g => g.GradeCategory).LoadAsync();
        return entity;
    }

    public async Task<Grade> UpdateAsync(Grade entity)
    {
        _context.Grades.Update(entity);
        await _context.SaveChangesAsync();

        // Recargar con la categoría incluida
        await _context.Entry(entity).Reference(g => g.GradeCategory).LoadAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var grade = await GetByIdAsync(id);
        if (grade == null) return false;

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Grades.AnyAsync(g => g.Id == id);
    }
}
