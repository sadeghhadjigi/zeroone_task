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

        public async Task<List<FlightDto>> GetFlightsByRoute(long originCityId, long destinationCityId)
        {
            return await _zeroOneDbContext.Flights.Include(x => x.Route)
                .Where(x => x.Route.OriginCityId == originCityId &&
                    x.Route.DestinationCityId == destinationCityId)
                .Select(x => new FlightDto
                {
                    AirlineId = x.AirlineId,
                    DepartureTime = x.DepartureTime,
                    ArrivalTime = x.ArrivalTime,
                    DestinationCityId = destinationCityId,
                    OriginCityId = originCityId,
                    FlightId = x.FlightId
                }).ToListAsync();
        }
    }
}