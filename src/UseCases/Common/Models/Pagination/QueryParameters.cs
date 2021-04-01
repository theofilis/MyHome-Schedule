using System.Collections.Generic;

namespace MyHome.Application.Common.Models.Pagination
{
    public class QueryParameters
    {
        public virtual int Page { get; set; } = 1;

        public virtual int Limit { get; set; } = 10;

        public virtual string SortField { get; set; } = "";

        public virtual string SortDir { get; set; } = "";

        protected List<string> _filter = new List<string>();

        protected List<object> _filterValue = new List<object>();

        public string GetFilters()
        {
            if (_filter.Count > 0)
                return string.Join(" and ", _filter);
            return "true";
        }

        public object[] GetFiltersValues()
            => _filterValue.ToArray();

        public void AddFilter(string field, string op, object value)
        {
            if (op.Equals("like"))
            {
                _filter.Add($"{field}.Contains(@{_filter.Count})");
                _filterValue.Add(value);
            }

            if (op.Equals("eq"))
            {
                _filter.Add($"{field} == @{_filter.Count}");
                _filterValue.Add(value);
            }
        }
    }
}
