using System.Text;
using ZeroOneTask.Application.Dtos;
using ZeroOneTask.Application.Interfaces;

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
                var flightsByRoute = inDateTotalFlights
                                        .GroupBy(x => x.OriginCityId)
                                        .ToDictionary(group => group.Key, group => group.GroupBy(x => x.DestinationCityId).ToDictionary(g => g.Key, g => g.ToList()));

                var flights = new List<FlightDto>();
                foreach (var subscription in agencySubscriptions)
                {
                    if (flightsByRoute.ContainsKey(subscription.OriginCityId))
                    {
                        if (flightsByRoute[subscription.OriginCityId].ContainsKey(subscription.DestinationCityId))
                        {
                            flights.AddRange(flightsByRoute[subscription.OriginCityId][subscription.DestinationCityId]);
                        }
                    }
                }

                var flightsByAirlineId = flights
                                            .GroupBy(x => x.AirlineId)
                                            .ToDictionary(group => group.Key, group => group.Select(x => x.DepartureTime).ToList());

                var validFlights = flights.Where(flight => flight.DepartureTime >= startDate && flight.DepartureTime <= endDate);
                foreach (var flight in validFlights)
                {
                    flight.Status = !flightsByAirlineId[flight.AirlineId].Any(f => f >= flight.DepartureTime.AddDays(-7).AddHours(-0.5) &&
                        f <= flight.DepartureTime.AddDays(-7).AddHours(0.5)) ? "New"
                        : !flightsByAirlineId[flight.AirlineId].Any(f => f >= flight.DepartureTime.AddDays(7).AddHours(-0.5) &&
                        f <= flight.DepartureTime.AddDays(7).AddHours(0.5)) ? "Discontinued"
                        : "";
                }

                var finalResult = flights.Where(f => !string.IsNullOrEmpty(f.Status)).ToList();

                var filePath = "result.csv";
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("flight_id,origin_city_id,destination_city_id,departure_time,arrival_time,airline_id,status");

                    foreach (var row in finalResult)
                    {
                        writer.WriteLine($"{row.FlightId},{row.OriginCityId},{row.DestinationCityId},{row.DepartureTime},{row.ArrivalTime},{row.AirlineId},{row.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}