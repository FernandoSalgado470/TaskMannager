using GradesService.Application.DTOs;
using GradesService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GradesService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGradesController : ControllerBase
{
    private readonly IStudentGradeService _studentGradeService;

    public StudentGradesController(IStudentGradeService studentGradeService)
    {
        _studentGradeService = studentGradeService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentGradeDto>>>> GetAll()
    {
        try
        {
            var grades = await _studentGradeService.GetAllStudentGradesAsync();
            return Ok(ApiResponse<IEnumerable<StudentGradeDto>>.SuccessResponse(grades, "Calificaciones finales obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<StudentGradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<StudentGradeDto>>> GetById(int id)
    {
        try
        {
            var grade = await _studentGradeService.GetStudentGradeByIdAsync(id);
            if (grade == null)
                return NotFound(ApiResponse<StudentGradeDto>.ErrorResponse("Calificación final no encontrada"));

            return Ok(ApiResponse<StudentGradeDto>.SuccessResponse(grade, "Calificación final obtenida exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGradeDto>.ErrorResponse($"Error al obtener calificación: {ex.Message}"));
        }
    }

    [HttpGet("student/{studentId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentGradeDto>>>> GetByStudentId(int studentId)
    {
        try
        {
            var grades = await _studentGradeService.GetStudentGradesByStudentIdAsync(studentId);
            return Ok(ApiResponse<IEnumerable<StudentGradeDto>>.SuccessResponse(grades, "Calificaciones del estudiante obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<StudentGradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpGet("subject/{subjectId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentGradeDto>>>> GetBySubjectId(int subjectId)
    {
        try
        {
            var grades = await _studentGradeService.GetStudentGradesBySubjectIdAsync(subjectId);
            return Ok(ApiResponse<IEnumerable<StudentGradeDto>>.SuccessResponse(grades, "Calificaciones de la materia obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<StudentGradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<StudentGradeDto>>> Create([FromBody] CreateStudentGradeDto createGradeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<StudentGradeDto>.ErrorResponse("Datos inválidos", errors));
            }

            var grade = await _studentGradeService.CreateStudentGradeAsync(createGradeDto);
            return CreatedAtAction(nameof(GetById), new { id = grade.Id }, ApiResponse<StudentGradeDto>.SuccessResponse(grade, "Calificación final creada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StudentGradeDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGradeDto>.ErrorResponse($"Error al crear calificación: {ex.Message}"));
        }
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<StudentGradeDto>>> Update(int id, [FromBody] CreateStudentGradeDto updateGradeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<StudentGradeDto>.ErrorResponse("Datos inválidos", errors));
            }

            var grade = await _studentGradeService.UpdateStudentGradeAsync(id, updateGradeDto);
            return Ok(ApiResponse<StudentGradeDto>.SuccessResponse(grade, "Calificación final actualizada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<StudentGradeDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGradeDto>.ErrorResponse($"Error al actualizar calificación: {ex.Message}"));
        }
    }

    [HttpPost("calculate")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<StudentGradeDto>>> CalculateFinalGrade(
        [FromQuery] int studentId,
        [FromQuery] int subjectId,
        [FromQuery] int periodId)
    {
        try
        {
            var grade = await _studentGradeService.CalculateFinalGradeAsync(studentId, subjectId, periodId);
            return Ok(ApiResponse<StudentGradeDto>.SuccessResponse(grade, "Calificación final calculada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StudentGradeDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<StudentGradeDto>.ErrorResponse($"Error al calcular calificación: {ex.Message}"));
        }
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var result = await _studentGradeService.DeleteStudentGradeAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Calificación final no encontrada"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Calificación final eliminada exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse($"Error al eliminar calificación: {ex.Message}"));
        }
    }
}
