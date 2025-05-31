//using System.Linq.Dynamic.Core;

using System.Reflection;
using System.Text;

namespace BookStore.Application.Abstractions.Extensions
{
    public static class RepositoryApplySortExtensions
    {

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> sources, string orderByQueryString)
        {

            if (string.IsNullOrEmpty(orderByQueryString))
            {
                return sources;
            }

            var orderParams = orderByQueryString.Trim().Split(",");

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            var orderQueryBuilder = new StringBuilder();
            
            foreach (var param in orderParams)
            {
                var propertyFromQueryName = param.Split(" ")[0];
                
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToLower()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return sources;
            //return string.IsNullOrWhiteSpace(orderQuery) ? sources : sources.OrderBy(orderQuery);
        }

    }
}