using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHome.Application.Common.Models.Pagination
{
    public class PageResult<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PageResult() : this(new List<T>(), 0, 0, 0)
        {
        }

        public PageResult(IList<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PageResult<T> ToPagedList(IEnumerable<T> source, QueryParameters parameters)
        {
            var count = source.Count();
            var items = source
                .Skip((parameters.Page - 1) * parameters.Limit)
                .Take(parameters.Limit)
                .ToList();

            return new PageResult<T>(items, count, parameters.Page, parameters.Limit);
        }
    }
}
