using BookStore.Domain.Orders;
using BookStore.Domain.Orders.ValueObjects.Ids;
using BookStore.SharedKernel.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Orders.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
               .HasColumnName("OrderId")
               .HasConversion(id => id.Value,
                              v => OrderId.Create(v))
               .ValueGeneratedNever();

        builder.Property(o => o.UserId)
               .IsRequired();

        builder.Property(o => o.OrderedDate)         
               .HasColumnType("datetimeoffset")        
               .IsRequired();

        builder.Property(o => o.Status)               
               .HasConversion(
                    v => v.ToString(),
                    s => Enum.Parse<OrderStatus>(s))
               .HasMaxLength(20)
               .IsRequired();



        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(p => p.FullName).HasMaxLength(150).IsRequired();
            sa.Property(p => p.Line1).HasMaxLength(250).IsRequired();
            sa.Property(p => p.Line2).HasMaxLength(250);
            sa.Property(p => p.City).HasMaxLength(100).IsRequired();
            sa.Property(p => p.State).HasMaxLength(100).IsRequired();
            sa.Property(p => p.PostalCode).HasMaxLength(20).IsRequired();
            sa.Property(p => p.Country).HasMaxLength(100).IsRequired();
        });

        builder.Property(o => o.ShippingCost)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Ignore(o => o.Total);

        builder.HasMany(o => o.Items)
               .WithOne()                           
               .HasForeignKey("OrderId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}