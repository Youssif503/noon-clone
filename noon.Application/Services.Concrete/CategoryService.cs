using noon.Application.DTOs;
using noon.Application.Helpers;
using noon.Application.Service.Contract;
using noon.Domain.Models;

namespace noon.Application.Services.Concrete;

public class CategoryService:ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Category>> getAllCategoriesAsync()
    {
        var result =
            await _unitOfWork.Categories.getAllAsync();
        
        return result;
    }

    public async Task<Category> getCategoryByIdAsync(int CategoryId)
    {
       var Category = await _unitOfWork.Categories.getByIdAsync(CategoryId);
       return Category;
    }

    public async Task<ResponseCategoryDto> addCategoryAsync(createCategoryDto createCategoryDto)
    {
        Category newCategory = new Category
        {
            Name = createCategoryDto.name
        };
        
        await _unitOfWork.Categories.addAsync(newCategory);
        await _unitOfWork.SaveChangesAsync();
        
        return new ResponseCategoryDto
        {
           Name = newCategory.Name,
           Id =  newCategory.Id
        };
    }

    public async Task<Response> updateCategory(int categoryId, updateCategoryDto categoryDto)
    {
        var existCategory = 
            await _unitOfWork.Categories.getByIdAsync(categoryId);
        existCategory.Name = categoryDto.Name;
        _unitOfWork.Categories.update(existCategory);
        await _unitOfWork.SaveChangesAsync();

        return new Response
        {
            IsSuccess = true,
            Message = "Category Updated Successfully"
        };
    }

    public async Task<Response> deleteCategory(int categoryId)
    {
        var existCategory = await
            _unitOfWork.Categories.getByIdAsync(categoryId);
        _unitOfWork.Categories.delete(existCategory);
        await _unitOfWork.SaveChangesAsync();
        
        return new Response
        {
            IsSuccess = true,
            Message = "Category Deleted Successfully"
        };
    }
}