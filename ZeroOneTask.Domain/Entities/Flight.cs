using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroOneTask.Domain.Entities
{
    [Table(name: "Flights")]
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "flight_id")]
        public long FlightId { get; set; }

        [Column(name: "departure_time")]
        public DateTime DepartureTime { get; set; }

        [Column(name: "arrival_time")]
        public DateTime ArrivalTime { get; set; }

        [Column(name: "route_id")]
        public long RouteId { get; set; }

        [ForeignKey(nameof(RouteId))]
        public virtual Route Route { get; set; }

        [Column(name: "airline_id")]
        public int AirlineId { get; set; }
    }
}