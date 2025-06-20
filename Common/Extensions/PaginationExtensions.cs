using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Disaster_Prediction_And_Alert_System_API.Common.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, BaseFilter filter, CancellationToken cancellationToken = default)
        {
            // Apply sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = query.ApplySorting(filter.SortBy, filter.OrderBy);
            }

            var total = await query.CountAsync(cancellationToken);

            var items = new List<T>();

            if (filter.IsAllItems)
            {
                filter.Page = 1;
                filter.PageSize = total;

                items = await query.ToListAsync(cancellationToken);
            }
            else
            {
                items = await query
                   .Skip((filter.Page - 1) * filter.PageSize)
                   .Take(filter.PageSize)
                   .ToListAsync(cancellationToken);
            }

            return new PagedResult<T>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = total
            };
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string sortBy, string? orderBy)
        {
            var propertyInfo = typeof(T).GetProperty(sortBy, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (propertyInfo == null)
            {
                // ถ้าไม่เจอ property ที่ระบุ ไม่ sort
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo.Name);
            var lambda = Expression.Lambda(property, parameter);

            string method = orderBy?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

            var expression = Expression.Call(
                typeof(Queryable),
                method,
                new Type[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(expression);
        }
    }
}
