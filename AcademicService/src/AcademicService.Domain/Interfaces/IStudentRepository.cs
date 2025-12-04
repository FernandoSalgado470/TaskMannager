using AcademicService.Domain.Entities;
using AcademicService.Domain.Interfaces; // Ajusta este using si tu IRepository está en otro namespace

namespace AcademicService.Domain.Interfaces // <-- ¡Este namespace es vital!
{
    
    public interface IStudentRepository : IRepository<Student>
    {
        
    }
}