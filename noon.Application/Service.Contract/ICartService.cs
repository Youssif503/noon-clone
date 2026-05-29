using noon.Application.DTOs.CartItem;
using noon.Application.Helpers;
using noon.Domain.Models;
namespace noon.Application.Service.Contract;
public interface ICartService
{
    Task<Response> AddCartItemAsync(AddCartItem cartItem);
    Task<List<CartItemDto>> GetCartItemsAsync(string userId);
    Task<CartItemDto?> GetCartItemAsync(string userId,int CartItemId);
    Task<Response> RemoveCartItem(string userId, int productId);
}