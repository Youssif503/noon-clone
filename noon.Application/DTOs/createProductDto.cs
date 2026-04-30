using noon.Domain.Models;

namespace noon.Application.DTOs;

public class createProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int StockCount { get; set; } 
    public List<ProductImage?> ProductImages { get; set; }
}