using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using noon.Application;
using noon.Application.DTOs;
using noon.Application.Repository.Contract;
using noon.Application.Service.Contract;
namespace noon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)  
        {
            _productService =  productService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> addProduct(createProductDto createProductDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _productService.addProductAsync(createProductDto);
            if(result == null)
                return BadRequest("Add Product Failed");
            
            return CreatedAtAction(nameof(getById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var products = await _productService.getAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> getById(int id)
        {
            var categories = await _productService.getProductByIdAsync(id);
            return Ok(categories);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deleteProduct(int id)
        {
            var result =  await _productService.deleteProduct(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            
            return Ok(result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> updateProduct(int id, updateProductDto updateProductDto)
        {
            var result = await _productService.updateProduct(id, updateProductDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            
            return Ok(result.Message);
        }
        
        
    }
}
