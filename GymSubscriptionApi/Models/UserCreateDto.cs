namespace GymSubscriptionApi.Models
{
    public class UserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int SubscriptionTypeId { get; set; }
        public int ServiceId { get; set; }
    }
}
