// ───────────────────────────────────────────────
//  BookId.cs   (BookStore.Domain.Books.ValueObjects)


using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.BookAuthors.ValueObjects;


    public class BookId : AggregateRootId<Guid>
    {
        private BookId() { }

        public sealed override Guid Value { get; protected set; }

        private BookId(Guid value)
        {
            Value = value;
        }

        public static BookId CreateUnique()
        {
            return new BookId(Guid.NewGuid());
        }

        public static BookId Create(Guid value)
        {
            return new BookId(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }