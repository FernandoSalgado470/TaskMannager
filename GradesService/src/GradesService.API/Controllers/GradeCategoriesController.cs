using GradesService.Application.DTOs;
using GradesService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GradesService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeCategoriesController : ControllerBase
{
    private readonly IGradeCategoryService _categoryService;

    public GradeCategoriesController(IGradeCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<GradeCategoryDto>>>> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(ApiResponse<IEnumerable<GradeCategoryDto>>.SuccessResponse(categories, "Categorías obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<GradeCategoryDto>>.ErrorResponse($"Error al obtener categorías: {ex.Message}"));
        }
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<GradeCategoryDto>>>> GetActive()
    {
        try
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(ApiResponse<IEnumerable<GradeCategoryDto>>.SuccessResponse(categories, "Categorías activas obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<GradeCategoryDto>>.ErrorResponse($"Error al obtener categorías: {ex.Message}"));
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GradeCategoryDto>>> GetById(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(ApiResponse<GradeCategoryDto>.ErrorResponse("Categoría no encontrada"));

            return Ok(ApiResponse<GradeCategoryDto>.SuccessResponse(category, "Categoría obtenida exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GradeCategoryDto>.ErrorResponse($"Error al obtener categoría: {ex.Message}"));
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GradeCategoryDto>>> Create([FromBody] CreateGradeCategoryDto createCategoryDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<GradeCategoryDto>.ErrorResponse("Datos inválidos", errors));
            }

            var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, ApiResponse<GradeCategoryDto>.SuccessResponse(category, "Categoría creada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<GradeCategoryDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GradeCategoryDto>.ErrorResponse($"Error al crear categoría: {ex.Message}"));
        }
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GradeCategoryDto>>> Update(int id, [FromBody] CreateGradeCategoryDto updateCategoryDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<GradeCategoryDto>.ErrorResponse("Datos inválidos", errors));
            }

            var category = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            return Ok(ApiResponse<GradeCategoryDto>.SuccessResponse(category, "Categoría actualizada exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<GradeCategoryDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GradeCategoryDto>.ErrorResponse($"Error al actualizar categoría: {ex.Message}"));
        }
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.ErrorResponse("Categoría no encontrada"));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Categoría eliminada exitosamente"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse($"Error al eliminar categoría: {ex.Message}"));
        }
    }
}
