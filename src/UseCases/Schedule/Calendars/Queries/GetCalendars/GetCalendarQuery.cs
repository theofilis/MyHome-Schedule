using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MyHome.Application.Common.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MyHome.Domain.Extensions;
using MyHome.Application.Common.Models.Pagination;
using MyHome.Application.Common.Services;

namespace MyHome.Application.Schedule.Calendars.Queries.GetCalendars
{
    public class GetCalendarsQuery : IRequest<PageResult<CalendarListDto>>
    {
        public QueryParameters Query { get; set; } = new QueryParameters();
    }

    public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, PageResult<CalendarListDto>>
    {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        private readonly ICurrentUserService _currentUserService;

        public GetCalendarsQueryHandler(IApplicationDbContext context,
                                           ICurrentUserService currentUserService,
                                           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PageResult<CalendarListDto>> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Calendar
                  .OwnedBy(_currentUserService.UserId)
                  .ProjectTo<CalendarListDto>(_mapper.ConfigurationProvider)
                  .Where(request.Query.GetFilters(), request.Query.GetFiltersValues())
                  .ApplySort(request.Query)
                  .ToPageListAsync(request.Query, cancellationToken);

            return result;
        }
    }
}
