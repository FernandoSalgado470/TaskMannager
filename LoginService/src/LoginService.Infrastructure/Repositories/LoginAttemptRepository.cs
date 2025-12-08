using LoginService.Domain.Entities;
using LoginService.Domain.Interfaces;
using LoginService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoginService.Infrastructure.Repositories;

public class LoginAttemptRepository : ILoginAttemptRepository
{
    private readonly LoginDbContext _context;

    public LoginAttemptRepository(LoginDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LoginAttempt>> GetAllAsync()
    {
        return await _context.LoginAttempts
            .Include(la => la.User)
            .ToListAsync();
    }

    public async Task<LoginAttempt?> GetByIdAsync(int id)
    {
        return await _context.LoginAttempts
            .Include(la => la.User)
            .FirstOrDefaultAsync(la => la.Id == id);
    }

    public async Task<IEnumerable<LoginAttempt>> GetByUserIdAsync(int userId)
    {
        return await _context.LoginAttempts
            .Where(la => la.UserId == userId)
            .OrderByDescending(la => la.AttemptedAt)
            .ToListAsync();
    }

    public async Task<int> GetFailedAttemptsCountAsync(int userId, TimeSpan timeWindow)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(timeWindow);
        return await _context.LoginAttempts
            .Where(la => la.UserId == userId && !la.IsSuccessful && la.AttemptedAt >= cutoffTime)
            .CountAsync();
    }

    public async Task<LoginAttempt> CreateAsync(LoginAttempt entity)
    {
        _context.LoginAttempts.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<LoginAttempt> UpdateAsync(LoginAttempt entity)
    {
        _context.LoginAttempts.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var attempt = await GetByIdAsync(id);
        if (attempt == null) return false;

        _context.LoginAttempts.Remove(attempt);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.LoginAttempts.AnyAsync(la => la.Id == id);
    }
}
