using GradesService.Application.DTOs;
using GradesService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GradesService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<GradeDto>>>> GetAll()
    {
        try
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return Ok(ApiResponse<IEnumerable<GradeDto>>.SuccessResponse(grades, "Calificaciones obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<GradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GradeDto>>> GetById(int id)
    {
        try
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
                return NotFound(ApiResponse<GradeDto>.ErrorResponse("Calificación no encontrada"));

            return Ok(ApiResponse<GradeDto>.SuccessResponse(grade, "Calificación obtenida exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GradeDto>.ErrorResponse($"Error al obtener calificación: {ex.Message}"));
        }
    }

    [HttpGet("student/{studentId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<GradeDto>>>> GetByStudentId(int studentId)
    {
        try
        {
            var grades = await _gradeService.GetGradesByStudentIdAsync(studentId);
            return Ok(ApiResponse<IEnumerable<GradeDto>>.SuccessResponse(grades, "Calificaciones del estudiante obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<GradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpGet("subject/{subjectId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<GradeDto>>>> GetBySubjectId(int subjectId)
    {
        try
        {
            var grades = await _gradeService.GetGradesBySubjectIdAsync(subjectId);
            return Ok(ApiResponse<IEnumerable<GradeDto>>.SuccessResponse(grades, "Calificaciones de la materia obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<GradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpGet("student/{studentId}/subject/{subjectId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<GradeDto>>>> GetByStudentAndSubject(int studentId, int subjectId)
    {
        try
        {
            var grades = await _gradeService.GetGradesByStudentAndSubjectAsync(studentId, subjectId);
            return Ok(ApiResponse<IEnumerable<GradeDto>>.SuccessResponse(grades, "Calificaciones obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<GradeDto>>.ErrorResponse($"Error al obtener calificaciones: {ex.Message}"));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GradeDto>>> Create([FromBody] CreateGradeDto createGradeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<GradeDto>.ErrorResponse("Datos inválidos", errors));
            }

            var grade = await _gradeService.CreateGradeAsync(createGradeDto);
            return CreatedAtAction(nameof(GetById), new { id = grade.Id }, ApiResponse<GradeDto>.SuccessResponse(grade, "Calificación creada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<GradeDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GradeDto>.ErrorResponse($"Error al crear calificación: {ex.Message}"));
        }
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GradeDto>>> Update(int id, [FromBody] CreateGradeDto updateGradeDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<GradeDto>.ErrorResponse("Datos inválidos", errors));
            }

            var grade = await _gradeService.UpdateGradeAsync(id, updateGradeDto);
            return Ok(ApiResponse<GradeDto>.SuccessResponse(grade, "Calificación actualizada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<GradeDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GradeDto>.ErrorResponse($"Error al actualizar calificación: {ex.Message}"));
        }
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var result = await _gradeService.DeleteGradeAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Calificación no encontrada"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Calificación eliminada exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse($"Error al eliminar calificación: {ex.Message}"));
        }
    }
}
