using ZeroOneTask.Application.Dtos;

namespace ZeroOneTask.Application.Interfaces
{
    public interface IFlightRepository
    {
        Task<List<FlightDto>> GetFlightsByRoute(long originCityId, long destinationCityId, DateTime startDate, DateTime endDate);
    }
}