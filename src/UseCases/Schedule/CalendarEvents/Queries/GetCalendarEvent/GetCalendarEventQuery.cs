using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyHome.Application.Common.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyHome.Application.Schedule.CalendarEvents.Queries.GetCalendarEvent
{
    public class GetCalendarEventQuery : IRequest<CalendarEventViewDto>
    {
        public long Id { get; set; }
    }

    public class GetCalendarEventQueryHandler : IRequestHandler<GetCalendarEventQuery, CalendarEventViewDto>
    {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        public GetCalendarEventQueryHandler(IApplicationDbContext context,
                                    ICurrentUserService currentUserService,
                                    IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CalendarEventViewDto> Handle(GetCalendarEventQuery request, CancellationToken cancellationToken)
        {
            return await _context.CalendarEvent
                .ProjectTo<CalendarEventViewDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id);
        }
    }
}
