// ───────────────────────────────────────────────
//  BookMappingConfig.cs   (BookStore.Application)
// ───────────────────────────────────────────────

using BookStore.Application.BookAuthors.Books.Commands.CreateBook;
using BookStore.Contracts.Books;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Entities;
using Mapster;

namespace BookStore.Application.BookAuthors.Mapping;

public class BookMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // CreateBookRequest → CreateBookCommand
        config.NewConfig<CreateBookRequest, CreateBookCommand>()
            .Map(disc => disc.Request, src => src)
            .IgnoreNullValues(true);

        config.NewConfig<UpdateBookRequest, CreateBookCommand>()
            .Map(disc => disc.Request, src => src)
            .IgnoreNullValues(true);

        // MappingConfig.cs (or wherever you register Mapster maps)

        config.NewConfig<Book, BookResponse>()
            // ─── Scalars ──────────────────────────────────────────────
            .Map(dest => dest.BookId, src => src.Id.Value.ToString())
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.IsAvailable, src => src.IsAvailable)
            .Map(dest => dest.StockQuantity, src => src.StockQuantity)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.PublishedDate, src => src.PublishedDate)
            .Map(dest => dest.Isbn, src => src.Isbn)
            .Map(dest => dest.CoverImageUrl, src => src.CoverImageUrl)
            .Map(dest => dest.CreatedDate, src => src.CreatedDate)

            .Map(dest => dest.AuthorsResponses,
                src => src.Authors
                    .Select(a => new BookAuthorsResponse(a.Name, a.Gender))
                    .ToList());

    }
}