namespace noon.Application.DTOs.CartItem;

public class CartItemDto
{ 
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
}