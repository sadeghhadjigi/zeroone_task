using ZeroOneTask.Application.Dtos;

namespace ZeroOneTask.Application.Interfaces
{
    public interface IFlightRepository
    {
        Task<List<FlightDto>> GetFlightsByDate(DateTime startDate, DateTime endDate);
    }
}