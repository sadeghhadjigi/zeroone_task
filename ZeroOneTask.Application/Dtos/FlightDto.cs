namespace ZeroOneTask.Application.Dtos
{
    public class FlightDto
    {
        public long FlightId { get; set; }

        public long OriginCityId { get; set; }

        public long DestinationCityId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int AirlineId { get; set; }

        public string Status { get; set; }
    }
}