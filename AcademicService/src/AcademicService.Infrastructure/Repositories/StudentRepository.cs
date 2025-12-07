using AcademicService.Domain.Entities;
using AcademicService.Domain.Interfaces;
using AcademicService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcademicService.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AcademicDbContext _context;

        public StudentRepository(AcademicDbContext context)
        {
            _context = context;
        }

        // --- Obtener estudiante por ID ---
        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        // --- Añadir estudiante con validaciones ---
        public async Task AddAsync(Student entity)
        {
            // Validación: solo letras en nombre
            if (!Regex.IsMatch(entity.FirstName, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s-]+$"))
                throw new Exception("El nombre solo puede contener letras, espacios o guiones.");

            // Validación: solo letras en apellido
            if (!Regex.IsMatch(entity.LastName, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s-]+$"))
                throw new Exception("El apellido solo puede contener letras, espacios o guiones.");

            // Validación: email duplicado
            bool emailExists = await EmailExistsAsync(entity.Email);
            if (emailExists)
                throw new Exception($"El correo '{entity.Email}' ya está registrado.");

            // Guardar en la base de datos
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // --- Método para verificar email duplicado ---
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Students.AnyAsync(s => s.Email == email);
        }
    }
}
