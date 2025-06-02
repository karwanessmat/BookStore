using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Entities;
using BookStore.Domain.BookAuthors.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.BookAuthors.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("BookId")
            .HasConversion(id => id.Value, v => BookId.Create(v))
            .IsRequired();

        builder.Property(b => b.CreatedDate).IsRequired();

        builder.Property(b => b.Title).HasMaxLength(500).IsRequired();
        builder.Property(b => b.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(b => b.StockQuantity).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(2000);
        builder.Property(b => b.PublishedDate);
        builder.Property(b => b.Isbn).HasMaxLength(20);
        builder.Property(b => b.CoverImageUrl).HasMaxLength(1000);
        builder.Property(b => b.IsAvailable).HasDefaultValueSql("1");




        builder.Navigation(x => x.Authors)
            .HasField("_authors")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Authors)
            .WithMany(a => a.Books)
            .UsingEntity<BookAuthor>(j =>
            {
                j.ToTable("BookAuthors");
                j.HasKey(e => new { e.BookId, e.AuthorId });

                j.Property(e => e.BookId)
                    .HasConversion(id => id.Value, v => BookId.Create(v))
                    .HasColumnName("BookId");      // FK column

                j.Property(e => e.AuthorId)
                    .HasConversion(id => id.Value, v => AuthorId.Create(v))
                    .HasColumnName("AuthorId");

                j.HasOne<Author>()
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                j.HasOne<Book>()
                    .WithMany()
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

    }
}
