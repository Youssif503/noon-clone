namespace noon.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int StockCount { get; set; } = 0;
        public double BasePrice { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? Brand { get; set; }
        public bool? IsDeleted { get; set; }
        public List<ProductImages>? ProductImages { get; set; } = new();
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
