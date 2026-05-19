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
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> addCategory([FromBody] createCategoryDto category)
        {
            if(!ModelState.IsValid)
                return  BadRequest(ModelState);
            
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> updateCategory(int id, updateCategoryDto updatedCategory)
        {
            var result = await _categoryService.updateCategory(id, updatedCategory);
            if(!result.IsSuccess)
                return NotFound(result.Message);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deleteCategory(int id)
        {
            var result = await _categoryService.deleteCategory(id);
            
            if(!result.IsSuccess)
                return NotFound(result.Message);
            
            return NoContent();
        }
    }
}
