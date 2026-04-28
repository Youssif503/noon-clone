using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noon.Domain.Models;
namespace noon.Infrastructure.Data.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasMany(c=>c.CartItems)
                   .WithOne(ci=>ci.Cart)
                   .HasForeignKey(ci=>ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c=>c.UserId)
                   .IsUnique();
            
        }
    }
}
