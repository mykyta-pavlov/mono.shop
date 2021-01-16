using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(180);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.HasOne(t => t.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId);
            builder.Property(p => p.SizeQuantityXxs).HasColumnType("integer");
            builder.Property(p => p.SizeQuantityXs).HasColumnType("integer");
            builder.Property(p => p.SizeQuantityS).HasColumnType("integer");
            builder.Property(p => p.SizeQuantityM).HasColumnType("integer");
            builder.Property(p => p.SizeQuantityL).HasColumnType("integer");
            builder.Property(p => p.SizeQuantityXl).HasColumnType("integer");
            builder.Property(p => p.SizeQuantityXxl).HasColumnType("integer");
        }
    }
}