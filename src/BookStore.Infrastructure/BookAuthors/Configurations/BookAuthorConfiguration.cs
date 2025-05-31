using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Entities;
using BookStore.Domain.BookAuthors.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.BookAuthors.Configurations;
public sealed class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> b)
    {
        b.ToTable("BookAuthors");
        b.HasKey(e => new { e.BookId, e.AuthorId });
    }
}