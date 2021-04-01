using Microsoft.EntityFrameworkCore;
using MyHome.Application.Common.Models.Pagination;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Common.Extensions
{
    public static class QuerableExtensions
    {
        public static PageResult<TSource> ToPageList<TSource>(this IQueryable<TSource> source, QueryParameters parameters)
        {
            var count = source.Count();
            var items = source
                .Skip((parameters.Page - 1) * parameters.Limit)
                .Take(parameters.Limit)
                .ToList();

            return new PageResult<TSource>(items, count, parameters.Page, parameters.Limit);
        }

        public static async Task<PageResult<TSource>> ToPageListAsync<TSource>(this IQueryable<TSource> source,
                                                                               QueryParameters parameters,
                                                                               CancellationToken token = default)
        {
            var count = await source.CountAsync(token);
            var items = await source
                .Skip((parameters.Page - 1) * parameters.Limit)
                .Take(parameters.Limit)
                .ToListAsync(token);

            return new PageResult<TSource>(items, count, parameters.Page, parameters.Limit);
        }

        public static IQueryable<TSource> ApplySort<TSource>(this IQueryable<TSource> entities, QueryParameters parameters)
        {
            if (!entities.Any())
                return entities;

            if (string.IsNullOrWhiteSpace(parameters.SortField))
                return entities;

            var orderParams = parameters.SortField
                .Trim()
                .Split(',');
            var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(param, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = parameters.SortDir.ToUpper().Equals("DESC") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return entities.OrderBy(orderQuery);
        }
    }
}
