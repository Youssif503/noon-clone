using noon.Application.DTOs;
using noon.Application.Helpers;
using noon.Domain.Models;
namespace noon.Application.Service.Contract;
public interface ICategoryService
{
    Task<IEnumerable<Category>> getAllCategoriesAsync();
    Task<Category> getCategoryByIdAsync(int CategoryId);
    Task<ResponseCategoryDto> addCategoryAsync(createCategoryDto createCategoryDto);
    Task<Response> updateCategory(int categoryId, updateCategoryDto categoryDto);
    Task<Response> deleteCategory(int categoryId);
}