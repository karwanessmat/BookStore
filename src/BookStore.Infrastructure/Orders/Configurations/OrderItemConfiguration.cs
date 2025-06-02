using BookStore.Domain.Orders.Entities;
using BookStore.Domain.Orders.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Orders.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasColumnName("OrderItemId")
            .HasConversion(id => id.Value,
                v => OrderItemId.Create(v))
            .ValueGeneratedNever();

        builder.HasOne(oi => oi.Book)
            .WithMany()                            
            .HasForeignKey("BookId")
            .OnDelete(DeleteBehavior.Restrict);   

        builder.Property(oi => oi.BookTitle)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Ignore(oi => oi.SubTotal);
    }
}