using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroOneTask.Domain.Entities
{
    [Table(name: "Subscriptions")]
    public class Subscription
    {
        public int AgencyId { get; set; }

        public long OriginCityId { get; set; }

        public long DestinationCityId { get; set; }
    }
}