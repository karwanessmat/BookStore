
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.Domain.ShoppingCards;
using BookStore.Domain.ShoppingCards.Entities;
using BookStore.Domain.ShoppingCards.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.ShoppingCards.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("CartId")
            .HasConversion(
                id => id.Value,
                v => CartId.Create(v))
            .ValueGeneratedNever();

        builder.Property(c => c.UserId)
            .IsRequired();
        
        builder.Ignore(c => c.Total);

        
        builder.HasMany(c => c.Items)
            .WithOne()                      
            .HasForeignKey("CartId")        
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasOne(ci => ci.Book)
            .WithMany()          
            .HasForeignKey("BookId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .HasColumnName("CartItemId")
            .HasConversion(
                id => id.Value,
                v => CartItemId.Create(v))
            .ValueGeneratedNever();

        builder.Property(ci => ci.BookTitle)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(ci => ci.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Ignore(ci => ci.SubTotal);
    }
}