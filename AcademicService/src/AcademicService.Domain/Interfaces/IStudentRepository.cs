using AcademicService.Domain.Entities;
using System.Threading.Tasks;

namespace AcademicService.Domain.Interfaces
{
    // Interfaz para el repositorio de Student
    public interface IStudentRepository
    {
        /// <summary>
        /// Busca un estudiante por su ID.
        /// </summary>
        /// <param name="id">ID del estudiante.</param>
        /// <returns>La entidad Student o null si no se encuentra.</returns>
        Task<Student> GetByIdAsync(int id);
        
        /// <summary>
        /// Añade una nueva entidad Student a la base de datos.
        /// </summary>
        /// <param name="entity">La entidad Student a añadir.</param>
        /// <returns>Una tarea completada.</returns>
        Task AddAsync(Student entity);

        /// <summary>
        /// Verifica si un correo electrónico ya está registrado en la base de datos.
        /// </summary>
        /// <param name="email">Correo electrónico a verificar.</param>
        /// <returns>True si existe, false si no.</returns>
        Task<bool> EmailExistsAsync(string email);

        // Aquí puedes añadir otros métodos específicos de Student si los necesitas.
    }
}
