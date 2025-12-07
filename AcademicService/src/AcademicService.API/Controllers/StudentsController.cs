using AcademicService.Application.DTOs;
using AcademicService.Domain.Entities;
using AcademicService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcademicService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // --- Obtener estudiante por ID ---
        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(typeof(StudentDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();

            var studentDto = new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                IsActive = student.IsActive,
                CreatedDate = student.CreatedDate
            };

            return Ok(studentDto);
        }

        // --- Crear estudiante ---
        [HttpPost]
        [ProducesResponseType(typeof(StudentDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Validación de nombres y apellidos (solo letras, espacios y guiones)
                if (!Regex.IsMatch(studentDto.FirstName, @"^[a-zA-Z\s-]+$"))
                    throw new Exception("El nombre solo puede contener letras, espacios o guiones.");
                if (!Regex.IsMatch(studentDto.LastName, @"^[a-zA-Z\s-]+$"))
                    throw new Exception("El apellido solo puede contener letras, espacios o guiones.");

                // Validación de email duplicado
                bool emailExists = await _studentRepository.EmailExistsAsync(studentDto.Email);
                if (emailExists)
                    throw new Exception($"El correo '{studentDto.Email}' ya está registrado.");

                // Mapear DTO a entidad
                var student = new Student
                {
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    Email = studentDto.Email,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                // Guardar en la base de datos
                await _studentRepository.AddAsync(student);

                // Mapear entidad a DTO para respuesta
                var studentToReturn = new StudentDto
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    IsActive = student.IsActive,
                    CreatedDate = student.CreatedDate
                };

                // Devolver 201 Created con el DTO
                return CreatedAtAction(nameof(GetStudentById), new { id = studentToReturn.Id }, studentToReturn);
            }
            catch (Exception ex)
            {
                // Devuelve JSON con error
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
