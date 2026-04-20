using Microsoft.EntityFrameworkCore;
using noon.Domain.Models;
namespace noon.Infrastructure.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.CartItems)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Product)
                      .WithMany()
                     .HasForeignKey(ci => ci.ProductId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
