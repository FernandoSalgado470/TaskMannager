using GradesService.Domain.Entities;
using GradesService.Domain.Interfaces;
using GradesService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GradesService.Infrastructure.Repositories;

public class GradeCategoryRepository : IGradeCategoryRepository
{
    private readonly GradesDbContext _context;

    public GradeCategoryRepository(GradesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GradeCategory>> GetAllAsync()
    {
        return await _context.GradeCategories.ToListAsync();
    }

    public async Task<IEnumerable<GradeCategory>> GetActiveAsync()
    {
        return await _context.GradeCategories
            .Where(gc => gc.IsActive)
            .ToListAsync();
    }

    public async Task<GradeCategory?> GetByIdAsync(int id)
    {
        return await _context.GradeCategories.FindAsync(id);
    }

    public async Task<GradeCategory?> GetByNameAsync(string name)
    {
        return await _context.GradeCategories
            .FirstOrDefaultAsync(gc => gc.Name == name);
    }

    public async Task<GradeCategory> CreateAsync(GradeCategory entity)
    {
        _context.GradeCategories.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<GradeCategory> UpdateAsync(GradeCategory entity)
    {
        _context.GradeCategories.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category == null) return false;

        _context.GradeCategories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.GradeCategories.AnyAsync(gc => gc.Id == id);
    }
}
