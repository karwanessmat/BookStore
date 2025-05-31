namespace BookStore.SharedKernel.Books;

public enum Gender
{
    Male = 1,
    Female = 2,
}

public enum BookOrderBy
{
    // Title-based sorting
    TitleAscending = 1,
    TitleDescending = 2,

    // Author-based sorting
    AuthorAscending = 3,
    AuthorDescending = 4,

    // Price-based sorting
    PriceAscending = 5,
    PriceDescending = 6,

    // StockQuantity-based sorting
    QuantityAscending = 7,
    QuantityDescending = 8
}