using System.Text.Json.Serialization;

namespace GymSubscriptionApi.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [JsonIgnore] // Prevent circular reference when serializing
        public User User { get; set; }

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
