using Microsoft.Extensions.Configuration;
using noon.Application.DTOs.CartItem;
using noon.Application.Helpers;
using noon.Application.Repository.Contract;
using noon.Application.Service.Contract;
using noon.Domain.Models;

namespace noon.Application.Services.Concrete;
public class CartService:ICartService
{
    /// <summary>
    /// addCartItem for adding item to Your Cart
    /// GetCartItems <!-- For Show Your Cart -->
    /// 
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ImageResolver _imageResolver;
    public CartService(IUnitOfWork unitOfWork,ImageResolver imageResolver,ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _imageResolver = imageResolver;
        _cacheService = cacheService;
    }
    public async Task<Response> AddCartItemAsync(AddCartItem cartItem)
    {
        if(cartItem == null)
            throw new ArgumentNullException(nameof(cartItem));

        var userCart = await 
            _unitOfWork.Carts.GetCartAsync(cartItem.UserId);
        
        if (userCart == null)
        {
            var Cart = new Cart()
            {
                UserId = cartItem.UserId,
            };
            await _unitOfWork.Carts.CreateCartAsync(Cart);

            var product =  await 
                _unitOfWork.Products.getByIdAsync(cartItem.ProductId);
            CartItem newItem = new CartItem()
            {
                ProductId = cartItem.ProductId,
                CartId = Cart.Id,
                Quantity = 1,
                unitPrice = product.BasePrice,
            };
            
            await _unitOfWork.Carts.CreateCartItemAsync(newItem);
        }
        else
        {
            var existItem = await
                _unitOfWork.Carts.GetCartItemAsync(cartItem.UserId, cartItem.ProductId);

            if (existItem == null) // there is an cart but not found this Product
            {
                var product =  await 
                    _unitOfWork.Products.getByIdAsync(cartItem.ProductId);
                
                CartItem newItem = new CartItem()
                {
                    ProductId = cartItem.ProductId,
                    CartId = userCart.Id,
                    Quantity = 1,
                    unitPrice = product.BasePrice,
                };
                
                await _unitOfWork.Carts.CreateCartItemAsync(newItem);
            }
            else
            {
                existItem.IncreaseQuantity();
            }
        }
        var result = await _unitOfWork.SaveChangesAsync();
        
        if (result > 0)
        {
            await _cacheService.RemoveAsync($"Cart-{cartItem.UserId}");
        }
        return new Response()
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Item successfully deleted" : "Item failed to delete"
        };
    }

    public async Task<List<CartItemDto>> GetCartItemsAsync(string userId)
    {
        var key = $"Cart-{userId}";
        var result = await _cacheService.GetAsync<List<CartItemDto>>(key);
        if (result != null)
        {
            return result;
        }
        
        var CartItems = await _unitOfWork
            .Carts.GetCartItemsAsync(userId);
        foreach (var item in CartItems)
        {
            item.ImageUrl = _imageResolver.Resolve(item.ImageUrl);
        }
        await _cacheService.SetAsync(key, CartItems);
        return CartItems;
    }

    public async Task<CartItemDto?> GetCartItemAsync(string userId, int CartItemId)
    {
        
        var CartItem = await 
            _unitOfWork.Carts.GetCartItemDtoAsync(userId, CartItemId);
        
        if(CartItem == null)
            throw new ArgumentNullException(nameof(CartItem));
        
        CartItem.ImageUrl = _imageResolver.Resolve(CartItem.ImageUrl);
        return CartItem;
    }

    public async Task<Response> RemoveCartItem(string userId, int productId)
    {
        var CartItem = await 
            _unitOfWork.Carts.GetCartItemAsync(userId, productId);
        
        if(CartItem == null)
            throw new KeyNotFoundException(nameof(CartItem));
        
        _unitOfWork.Carts.RemoveCartItem(CartItem);
        var result = await _unitOfWork.SaveChangesAsync();
        
        if (result > 0)
        {
            await _cacheService.RemoveAsync($"Cart-{userId}");
        }

        return new Response()
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Item successfully deleted" : "Item failed to delete"
        };
    }
    
}