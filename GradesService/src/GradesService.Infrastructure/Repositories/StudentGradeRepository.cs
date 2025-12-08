using GradesService.Domain.Entities;
using GradesService.Domain.Interfaces;
using GradesService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GradesService.Infrastructure.Repositories;

public class StudentGradeRepository : IStudentGradeRepository
{
    private readonly GradesDbContext _context;

    public StudentGradeRepository(GradesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentGrade>> GetAllAsync()
    {
        return await _context.StudentGrades.ToListAsync();
    }

    public async Task<StudentGrade?> GetByIdAsync(int id)
    {
        return await _context.StudentGrades.FindAsync(id);
    }

    public async Task<IEnumerable<StudentGrade>> GetByStudentIdAsync(int studentId)
    {
        return await _context.StudentGrades
            .Where(sg => sg.StudentId == studentId)
            .OrderByDescending(sg => sg.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<StudentGrade>> GetBySubjectIdAsync(int subjectId)
    {
        return await _context.StudentGrades
            .Where(sg => sg.SubjectId == subjectId)
            .OrderByDescending(sg => sg.CreatedAt)
            .ToListAsync();
    }

    public async Task<StudentGrade?> GetByStudentSubjectAndPeriodAsync(int studentId, int subjectId, int periodId)
    {
        return await _context.StudentGrades
            .FirstOrDefaultAsync(sg =>
                sg.StudentId == studentId &&
                sg.SubjectId == subjectId &&
                sg.AcademicPeriodId == periodId);
    }

    public async Task<IEnumerable<StudentGrade>> GetByAcademicPeriodAsync(int periodId)
    {
        return await _context.StudentGrades
            .Where(sg => sg.AcademicPeriodId == periodId)
            .OrderBy(sg => sg.StudentId)
            .ThenBy(sg => sg.SubjectId)
            .ToListAsync();
    }

    public async Task<StudentGrade> CreateAsync(StudentGrade entity)
    {
        _context.StudentGrades.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<StudentGrade> UpdateAsync(StudentGrade entity)
    {
        _context.StudentGrades.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var studentGrade = await GetByIdAsync(id);
        if (studentGrade == null) return false;

        _context.StudentGrades.Remove(studentGrade);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.StudentGrades.AnyAsync(sg => sg.Id == id);
    }
}
