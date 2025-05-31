using BookStore.Application.Abstractions.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Abstractions.Helpers;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }


    // it is created to config mapster.
    public PagedList(int totalCount, int pageNumber, int pageSize)
    {
        MetaData = new MetaData()
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
    public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        MetaData = new MetaData()
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        AddRange(items);
    }

    public static async Task<PagedList<T>> ToCreatePageListAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var totalCount = source.Count();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }
}
