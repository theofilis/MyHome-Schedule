using Microsoft.AspNetCore.Mvc;
using MyHome.Application.Common.Models.Pagination;
using MyHome.Dashboard.Binders;

namespace MyHome.Dashboard.ViewModels
{
    [ModelBinder(BinderType = typeof(SmartTableQueryParametersBinder))]
    public class SmartTableQueryParameters : QueryParameters
    {
        [FromQuery(Name = "_page")]
        public override int Page { get; set; } = 1;

        [FromQuery(Name = "_limit")]
        public override int Limit { get; set; } = 10;

        [FromQuery(Name = "_sort")]
        public override string SortField { get; set; } = "Id";

        [FromQuery(Name = "_order")]
        public override string SortDir { get; set; } = "DESC";
    }
}
