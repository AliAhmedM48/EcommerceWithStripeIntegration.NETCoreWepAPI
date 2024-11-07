using Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Data.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");

        builder.Property(o => o.Status)
            .HasConversion(p => p.ToString(), p => (OrderStatus)Enum.Parse(typeof(OrderStatus), p));

        builder.OwnsOne(p => p.ShippingAddress, sa => sa.WithOwner());

        builder.HasOne(p => p.DeliveryMethod).WithMany().HasForeignKey(p => p.DeliveryMethodId).OnDelete(DeleteBehavior.SetNull);
    }
}
