using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.BookAuthors.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("AuthorId")
            .HasConversion(id => id.Value, v => AuthorId.Create(v))
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(a => a.CreatedDate).IsRequired();

        builder.Property(a => a.Name).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Gender).HasConversion<int>().IsRequired();
        builder.Property(a => a.Bio).HasMaxLength(2000);

        // back-field navigation
        builder.Navigation(a => a.Books)
            .HasField("_books")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}