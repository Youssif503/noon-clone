using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noon.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace noon.Infrastructure.Data.Configuration
{
    public class ReViewConfiguration : IEntityTypeConfiguration<ReView>
    {
        public void Configure(EntityTypeBuilder<ReView> builder)
        {
            builder.HasOne(r => r.User)
                .WithMany(u => u.ReViews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

               builder.HasOne(r => r.Product)
                .WithMany(p => p.ReViews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.ReviewRate)
                .IsRequired()
                .HasMaxLength(5);
        }
    }
}
