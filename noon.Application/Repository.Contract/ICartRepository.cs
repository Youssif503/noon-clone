using noon.Application.DTOs.CartItem;
using noon.Domain.Models;
namespace noon.Application.Repository.Contract;

public interface ICartRepository
{
    Task CreateCartAsync(Cart cart);
    void RemoveCartItem(CartItem cartItem);
    Task CreateCartItemAsync(CartItem cartitem);
    void RemoveCart(Cart cart);
    Task<CartItemDto> GetCartItemDtoAsync(string userId, int cartItemId);
    Task<CartItem?> GetCartItemAsync(string userId, int cartItemId);
    Task<Cart?> GetCartAsync(string userId);
    Task<List<CartItemDto>> GetCartItemsAsync(string userId);

}