namespace noon.Application.DTOs.Product;

public class ResponseProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Id { get; set; }
    public double BasePrice { get; set; }
    public int StockCount { get; set; }
    public List<string> Images { get; set; }
    
}