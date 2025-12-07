using AcademicService.Domain.Interfaces;
using AcademicService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AcademicService.Domain.Entities; // Necesario si las entidades base están aquí

namespace AcademicService.Infrastructure.Repositories
{
    // Clase base genérica que implementa IRepository<T>
    public class Repository<T> : IRepository<T> where T : class
    {
        // Protected para que las clases hijas puedan acceder
        protected readonly AcademicDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AcademicDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Implementación de IRepository<T>

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
            // Nota: Uso EF.Property para acceder a la propiedad 'Id' de la entidad genérica T
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            // Adjuntar la entidad para marcarla como modificada.
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}