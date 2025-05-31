namespace BookStore.Application.Abstractions.Utilities;

public class MetaData
{
    public int PageNumber { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}
