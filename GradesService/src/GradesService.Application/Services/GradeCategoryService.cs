using GradesService.Application.DTOs;
using GradesService.Application.Interfaces;
using GradesService.Domain.Entities;
using GradesService.Domain.Interfaces;

namespace GradesService.Application.Services;

public class GradeCategoryService : IGradeCategoryService
{
    private readonly IGradeCategoryRepository _categoryRepository;

    public GradeCategoryService(IGradeCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<GradeCategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(MapToDto);
    }

    public async Task<IEnumerable<GradeCategoryDto>> GetActiveCategoriesAsync()
    {
        var categories = await _categoryRepository.GetActiveAsync();
        return categories.Select(MapToDto);
    }

    public async Task<GradeCategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category != null ? MapToDto(category) : null;
    }

    public async Task<GradeCategoryDto> CreateCategoryAsync(CreateGradeCategoryDto createCategoryDto)
    {
        // Validar que el nombre sea único
        var existingCategory = await _categoryRepository.GetByNameAsync(createCategoryDto.Name);
        if (existingCategory != null)
        {
            throw new InvalidOperationException("Ya existe una categoría con ese nombre");
        }

        var category = new GradeCategory
        {
            Name = createCategoryDto.Name,
            Description = createCategoryDto.Description,
            WeightPercentage = createCategoryDto.WeightPercentage,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdCategory = await _categoryRepository.CreateAsync(category);
        return MapToDto(createdCategory);
    }

    public async Task<GradeCategoryDto> UpdateCategoryAsync(int id, CreateGradeCategoryDto updateCategoryDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new InvalidOperationException("Categoría no encontrada");
        }

        // Validar que el nombre sea único (excepto para la categoría actual)
        var existingCategory = await _categoryRepository.GetByNameAsync(updateCategoryDto.Name);
        if (existingCategory != null && existingCategory.Id != id)
        {
            throw new InvalidOperationException("Ya existe una categoría con ese nombre");
        }

        category.Name = updateCategoryDto.Name;
        category.Description = updateCategoryDto.Description;
        category.WeightPercentage = updateCategoryDto.WeightPercentage;

        var updatedCategory = await _categoryRepository.UpdateAsync(category);
        return MapToDto(updatedCategory);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }

    private GradeCategoryDto MapToDto(GradeCategory category)
    {
        return new GradeCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            WeightPercentage = category.WeightPercentage,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt
        };
    }
}
