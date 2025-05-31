using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

// ReSharper disable InconsistentNaming

namespace BookStore.Infrastructure.Abstractions.Extensions;

public static class IQueryableExtensions
{
    // Extension method to include multiple navigation properties
    public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[]? includes) where T : class
    {
        return includes is null
            ? query
            : includes.Aggregate(query, (current, include) => current.Include(include));
    }
}


