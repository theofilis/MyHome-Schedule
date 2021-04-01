using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyHome.Application.Common.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.Calendars.Queries.GetCalendar
{
    public class GetCalendarQuery : IRequest<CalendarViewDto>
    {
        public long Id { get; set; }
    }

    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarViewDto>
    {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        public GetCalendarQueryHandler(IApplicationDbContext context,
                                    ICurrentUserService currentUserService,
                                    IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CalendarViewDto> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            return await _context.Calendar
                .ProjectTo<CalendarViewDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id);
        }
    }
}
