using AcademicService.Application.DTOs;
using AcademicService.Domain.Entities;
using AcademicService.Domain.Interfaces; // <-- Este using es vital
using Microsoft.AspNetCore.Mvc;

namespace AcademicService.API.Controllers
{
    [ApiController]
    [Route("api/students")] // <-- URL correcta
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        // Usar la interfaz IStudentRepository, no IStudentService
        private readonly IStudentRepository _repository; 

        public StudentsController(IStudentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene todos los estudiantes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentDto>>>> GetAll()
        {
            var students = await _repository.GetAllAsync();
            var dtos = students.Select(MapToDto);

            return Ok(ApiResponse<IEnumerable<StudentDto>>.SuccessResponse(dtos));
        }

        /// <summary>
        /// Crea un nuevo estudiante
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<StudentDto>>> Create([FromBody] CreateStudentDto dto)
        {
            var student = new Student
            {
                FirstName = dto.FirstName, // Asegúrate que CreateStudentDto tenga estas propiedades
                LastName = dto.LastName,
                Email = dto.Email,
                IsActive = true // Asumimos que IsActive es un campo del modelo de Estudiante
            };

            var created = await _repository.CreateAsync(student);

            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponse<StudentDto>.SuccessResponse(MapToDto(created), "Estudiante creado exitosamente"));
        }

        // --- MÉTODOS DE SOPORTE ---
        // (Debes tener un método GetById, Put, Delete, etc., similares a los del controlador de matrícula)

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<StudentDto>>> GetById(int id)
        {
             var student = await _repository.GetByIdAsync(id);
             if (student == null)
                 return NotFound(ApiResponse<StudentDto>.ErrorResponse("Estudiante no encontrado"));
             return Ok(ApiResponse<StudentDto>.SuccessResponse(MapToDto(student)));
        }

        private static StudentDto MapToDto(Student student)
        {
            // Crea esta clase en Application/DTOs/StudentDto.cs
            return new StudentDto 
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                IsActive = student.IsActive
            };
        }
    }
}