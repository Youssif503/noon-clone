using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            _productService = productService;
            _logger = logger;
        }
        
    }
}
