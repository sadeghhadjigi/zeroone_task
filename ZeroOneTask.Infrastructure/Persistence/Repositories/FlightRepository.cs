using Microsoft.EntityFrameworkCore;
using ZeroOneTask.Application.Dtos;
using ZeroOneTask.Application.Interfaces;

namespace ZeroOneTask.Infrastructure.Persistence.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ZeroOneTaskDbContext _zeroOneDbContext;

        public FlightRepository(ZeroOneTaskDbContext zeroOneTaskDbContext)
        {
            _zeroOneDbContext = zeroOneTaskDbContext;
        }

        public async Task<List<FlightDto>> GetFlightsByDate(DateTime startDate, DateTime endDate)
        {
            return await _zeroOneDbContext.Flights.Include(x => x.Route)
                .Where(x => x.Route.DepartureDate >= startDate.AddDays(-8) &&
                    x.Route.DepartureDate <= endDate.AddDays(8))
                .Select(x => new FlightDto
                {
                    AirlineId = x.AirlineId,
                    DepartureTime = x.DepartureTime,
                    ArrivalTime = x.ArrivalTime,
                    DestinationCityId = x.Route.DestinationCityId,
                    OriginCityId = x.Route.OriginCityId,
                    FlightId = x.FlightId
                }).ToListAsync();
        }
    }
}