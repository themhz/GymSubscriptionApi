using System.Text.Json.Serialization;

namespace GymSubscriptionApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        [JsonIgnore] // Prevent the password from being serialized
        public string Password { get; set; } // Store hashed password
        [JsonIgnore]
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
