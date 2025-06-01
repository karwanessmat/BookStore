using BookStore.Application.BookAuthors.Authors.Commands.CreateAuthor;
using BookStore.Application.BookAuthors.Authors.Commands.UpdateAuthor;
using BookStore.Contracts.Authors;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Books;
using Mapster;

namespace BookStore.Application.BookAuthors.Mapping;

public class AuthorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAuthorRequest, CreateAuthorCommand>()
            .Map(dest => dest.Request, src => src)
            .IgnoreNullValues(true);

        config.NewConfig<UpdateAuthorRequest, UpdateAuthorCommand>()
            .Map(dest => dest.Request, src => src)
            .IgnoreNullValues(true);

        config.NewConfig<Author, AuthorResponse>()
            .Map(dest => dest.AuthorId, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Gender, src => src.Gender)
            .Map(dest => dest.Bio, src => src.Bio)

            .Map(dest => dest.BooksResponses,
                src => src.Books
                    .Select(b => new AuthorBooksResponse(
                        b.Id.Value,
                        b.Title))
                    .ToList()
            );



        config.NewConfig<AuthorResponseFromSql, Author>()
            .Ignore(dest => dest.Books)
            .ConstructUsing(src => Author.Create(
                src.CreatedDate,
                src.Name,
                src.Gender == 1 ? Gender.Male : Gender.Female,
                src.Bio,
                AuthorId.Create(src.AuthorId)
            ));






    }
}