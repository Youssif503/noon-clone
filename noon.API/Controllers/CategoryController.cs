using Microsoft.AspNetCore.Mvc;
using noon.Application.DTOs;
using noon.Application.Service.Contract;
namespace noon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> addCategory([FromBody] createCategoryDto category)
        {
            var result = await _categoryService.addCategoryAsync(category);
            if (result ==null)
            {
                return BadRequest("Add Category Failed");
            }
            return CreatedAtAction(nameof(getById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> getallCategories()
        {
            var categories = 
                await _categoryService.getAllCategoriesAsync();
            
            return Ok(categories);
        }
        [HttpGet("/getById/{id:int}")]
        public async Task<IActionResult> getById(int id)
        {
            var result = await _categoryService.getCategoryByIdAsync(id);
            return Ok(result);
        }
        
        
    }
}
