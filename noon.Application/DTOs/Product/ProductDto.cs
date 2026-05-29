using noon.Application.DTOs.Product;
using noon.Application.DTOs.Review;
using noon.Domain.Models;
namespace noon.Application.DTOs;
public class ProductDto
{
    public int Id{ get;set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public List<ProductImageDto?> ProductImages { get; set; }
    public List<ReviewDto>? Reviews { get; set; }
}