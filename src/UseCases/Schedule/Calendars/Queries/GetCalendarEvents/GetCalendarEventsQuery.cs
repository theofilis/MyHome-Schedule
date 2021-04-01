using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MyHome.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MyHome.Application.Common.Services;

namespace MyHome.Application.Schedule.Calendars.Queries.GetCalendarEvents
{
    public class GetCalendarEventsQuery : IRequest<List<CalendarEventListDto>>
    {
        public long CalendarId { get; set; }
    }

    public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, List<CalendarEventListDto>>
    {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        private readonly ICurrentUserService _currentUserService;

        public GetCalendarEventsQueryHandler(IApplicationDbContext context,
                                           ICurrentUserService currentUserService,
                                           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<CalendarEventListDto>> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.CalendarEvent
                  .OwnedBy(_currentUserService.UserId)
                  .Where(e => Equals(e.CalendarId, request.CalendarId))
                  .OrderBy("Start")
                  .ProjectTo<CalendarEventListDto>(_mapper.ConfigurationProvider)
                  .ToListAsync();

            return result;
        }
    }
}
