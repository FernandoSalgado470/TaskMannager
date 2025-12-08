using GradesService.Application.DTOs;

namespace GradesService.Application.Interfaces;

public interface IGradeCategoryService
{
    Task<IEnumerable<GradeCategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<GradeCategoryDto>> GetActiveCategoriesAsync();
    Task<GradeCategoryDto?> GetCategoryByIdAsync(int id);
    Task<GradeCategoryDto> CreateCategoryAsync(CreateGradeCategoryDto createCategoryDto);
    Task<GradeCategoryDto> UpdateCategoryAsync(int id, CreateGradeCategoryDto updateCategoryDto);
    Task<bool> DeleteCategoryAsync(int id);
}
