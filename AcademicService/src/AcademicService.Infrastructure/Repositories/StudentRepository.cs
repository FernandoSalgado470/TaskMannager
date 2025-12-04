using AcademicService.Domain.Entities;
using AcademicService.Domain.Interfaces;
using AcademicService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AcademicService.Infrastructure.Repositories
{
    // Hereda de la clase base Repository genérica e implementa la interfaz específica.
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        // El contexto de la base de datos se pasa al constructor de la clase base (Repository<T>)
        public StudentRepository(AcademicDbContext context) : base(context)
        {
        }

        
    }
}