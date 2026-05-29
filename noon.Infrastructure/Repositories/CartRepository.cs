using Microsoft.EntityFrameworkCore;
using noon.Application.DTOs.CartItem;
using noon.Application.Repository.Contract;
using noon.Domain.Models;
using noon.Infrastructure.Data;
namespace noon.Infrastructure.Repositories;

public class CartRepository:ICartRepository
{
    private readonly ApplicationDbContext _context;
    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task CreateCartItemAsync(CartItem cartitem)
    {
        await _context.CartItems.AddAsync(cartitem);
    }

    public async Task<CartItemDto?> GetCartItemDtoAsync(string userId, int cartItemId)
    {
        return await _context.CartItems
            .Where(ci => ci.Id == cartItemId && ci.Cart.UserId == userId)
            .Select(ci => new CartItemDto()
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.unitPrice,
                Quantity = ci.Quantity,
                ImageUrl = ci.Product.ProductImages.
                    Select(pi => pi.ImageUrl).FirstOrDefault()
            }).FirstOrDefaultAsync();
    }

    public async Task<CartItem?> GetCartItemAsync(string userId, int cartItemId)
    {
        return await _context.CartItems
            .Where(ci => ci.Id == cartItemId && ci.Cart.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Cart?> GetCartAsync(string userId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public void RemoveCart(Cart cart)
    {
        _context.Carts.Remove(cart);
    }

    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task<List<CartItemDto>> GetCartItemsAsync(string userId)
    {
        return await _context.CartItems
            .Where(ci => ci.Cart.UserId == userId)
            .Select(ci => new CartItemDto()
            {
                ProductId =  ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.unitPrice,
                Quantity = ci.Quantity,
                ImageUrl = ci.Product.ProductImages.
                    Select(pi=>pi.ImageUrl).FirstOrDefault()
            }).ToListAsync();
    }
}
