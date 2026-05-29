using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using noon.Application.DTOs.CartItem;
using noon.Application.Service.Contract;

namespace noon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;
        public CartController(
            ILogger<CartController> logger, 
            ICartService cartService
            )
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.Claims.FirstOrDefault(c=>c.Type=="userId")?.Value;
            if(userId == null)
                return Unauthorized();
            
            var result = await _cartService.GetCartItemsAsync(userId);
            
            return Ok(result);
        }

        [HttpGet("{ItemId:int}")]
        [Authorize]
        public async Task<IActionResult> GetCartItem(int ItemId)
        {
            var userId = User.Claims.FirstOrDefault(c=>c.Type=="userId")?.Value;
            if(userId == null)
                return Unauthorized();
            
            
            var result = await _cartService.GetCartItemAsync(userId, ItemId);
            
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItem cartItem)
        {
            var userId = User.Claims.FirstOrDefault(c=>c.Type=="userId")?.Value;
            if(userId == null)
                return Unauthorized();
            
            cartItem.UserId = userId;
            
            var result = await _cartService.AddCartItemAsync(cartItem);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            
            return Ok(result);
        }
        
        [HttpDelete("{productId:int}")]
        [Authorize]
        public async Task<IActionResult> RemoveCartItem(int productId)
        {
            var userId = User.FindFirst("userId")?.Value;

            if(userId == null)
                return Unauthorized();

            var result = await _cartService.RemoveCartItem(userId, productId);

            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
