using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroOneTask.Domain.Entities
{
    [Table(name: "Routes")]
    public class Route
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "route_id")]
        public long RouteId { get; set; }

        [Column(name: "departure_date")]
        public DateTime DepartureDate { get; set; }

        [Column(name: "origin_city_id")]
        public long OriginCityId { get; set; }

        [Column(name: "destination_city_id")]
        public long DestinationCityId { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
    }
}