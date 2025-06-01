using BookStore.Domain.BookAuthors.Entities;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Books;

namespace BookStore.Domain.BookAuthors;

public sealed class Author : AggregateRoot<AuthorId, Guid>
{
    public DateTimeOffset CreatedDate { get; }
    public string Name { get; private set; }
    public Gender Gender { get; private set; }
    public string? Bio { get; private set; }

    private readonly List<Book> _books = new();
    public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

    private Author() { } 

    private Author(DateTimeOffset created, string name,
        Gender gender, string? bio, AuthorId? id = null)
        : base(id ?? AuthorId.CreateUnique())
    {
        CreatedDate = created;
        Name = name.Trim();
        Gender = gender;
        Bio = bio?.Trim();
    }

    public static Author Create(
        DateTimeOffset created, 
        string name,
        Gender gender, 
        string? bio = null,
        AuthorId? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Author name cannot be empty.", nameof(name));

        return new Author(created, name, gender, bio, id);
    }

    public void Update(string newName, Gender newGender, string? newBio)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Author name cannot be empty.", nameof(newName));

        Name = newName.Trim();
        Gender = newGender;
        Bio = newBio?.Trim();
    }

    // Domain convenience
    public void AddBook(Book id) => _books.Add(id);
    public void RemoveBook(Book id) => _books.Remove(id);
}
