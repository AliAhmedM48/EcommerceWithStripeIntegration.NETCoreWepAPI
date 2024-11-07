using Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Data.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.Property(p => p.PictureUrl).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal(12,2)");

        builder.Property(p => p.BrandId).IsRequired(false);
        builder.Property(p => p.TypeId).IsRequired(false);

        builder.HasOne(p => p.Brand).WithMany().HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(p => p.Type).WithMany().HasForeignKey(p => p.TypeId).OnDelete(DeleteBehavior.SetNull);
    }
}
