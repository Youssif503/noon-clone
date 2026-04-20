using Microsoft.EntityFrameworkCore;
using noon.Domain.Models;

namespace noon.Infrastructure.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

             builder.Property(c => c.Name)
                 .IsRequired()
                 .HasMaxLength(255);
        }
    }
}
