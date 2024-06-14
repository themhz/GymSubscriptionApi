namespace GymSubscriptionApi.Models
{
    public class SubscriptionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; } // E.g., 1 for daily, 30 for monthly, etc.
    }
}
