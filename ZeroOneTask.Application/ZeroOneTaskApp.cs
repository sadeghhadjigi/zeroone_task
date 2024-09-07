using System.Text;
using ZeroOneTask.Application.Dtos;
using ZeroOneTask.Application.Interfaces;
using ZeroOneTask.Domain.Entities;

namespace ZeroOneTask.Application
{
    public class ZeroOneTaskApp
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFlightRepository _flightRepository;

        public ZeroOneTaskApp(ISubscriptionRepository subscriptionRepository, IFlightRepository flightRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _flightRepository = flightRepository;
        }

        public async Task Run(DateTime startDate, DateTime endDate, int agencyId)
        {
            try
            {
                var agencySubscriptions = await _subscriptionRepository.GetSubscriptionsByAgencyId(agencyId);
                var inDateTotalFlights = await _flightRepository.GetFlightsByDate(startDate, endDate);
                var flights = PrepareAgencyFlights(agencySubscriptions, inDateTotalFlights);
                var results = PrepareChangeDetectionFlights(flights, startDate, endDate);
                GenerateCSV("result.csv", results);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<FlightDto> PrepareAgencyFlights(List<Subscription> agencySubscriptions, List<FlightDto> inDateTotalFlights)
        {
            var flightsByRouteDict = inDateTotalFlights
                    .GroupBy(x => x.OriginCityId)
                    .ToDictionary(group => group.Key, group => group.GroupBy(x => x.DestinationCityId).ToDictionary(g => g.Key, g => g.ToList()));

            var flights = new List<FlightDto>();
            foreach (var subscription in agencySubscriptions)
            {
                if (flightsByRouteDict.ContainsKey(subscription.OriginCityId) && flightsByRouteDict[subscription.OriginCityId].ContainsKey(subscription.DestinationCityId))
                {
                    flights.AddRange(flightsByRouteDict[subscription.OriginCityId][subscription.DestinationCityId]);
                }
            }

            return flights;
        }

        private List<FlightDto> PrepareChangeDetectionFlights(List<FlightDto> flights, DateTime startDate, DateTime endDate)
        {
            var flightsByAirlineIdDict = flights
                    .GroupBy(x => x.AirlineId)
                    .ToDictionary(group => group.Key, group => new HashSet<DateTime>(group.Select(x => x.DepartureTime)));

            var validFlights = flights.Where(flight => flight.DepartureTime >= startDate && flight.DepartureTime <= endDate);
            Parallel.ForEach(validFlights, flight =>
            {
                var lastWeekFlightDepartureTime = flight.DepartureTime.AddDays(-7);
                var nextWeekFlightDepartureTime = flight.DepartureTime.AddDays(7);

                flight.Status = !flightsByAirlineIdDict[flight.AirlineId].Any(f => f >= lastWeekFlightDepartureTime.AddHours(-0.5) &&
                    f <= lastWeekFlightDepartureTime.AddHours(0.5)) ? "New"
                    : !flightsByAirlineIdDict[flight.AirlineId].Any(f => f >= nextWeekFlightDepartureTime.AddHours(-0.5) &&
                    f <= nextWeekFlightDepartureTime.AddHours(0.5)) ? "Discontinued"
                    : "";
            });

            return validFlights.Where(f => !string.IsNullOrEmpty(f.Status)).ToList();
        }

        private void GenerateCSV(string filePath, List<FlightDto> results)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("flight_id,origin_city_id,destination_city_id,departure_time,arrival_time,airline_id,status");

                foreach (var row in results)
                {
                    writer.WriteLine($"{row.FlightId},{row.OriginCityId},{row.DestinationCityId},{row.DepartureTime},{row.ArrivalTime},{row.AirlineId},{row.Status}");
                }
            }
        }
    }
}