using GymSubscriptionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSubscriptionApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Subscriptions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.SubscriptionType)
                .WithMany()
                .HasForeignKey(s => s.SubscriptionTypeId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Service)
                .WithMany()
                .HasForeignKey(s => s.ServiceId);

            modelBuilder.Entity<SubscriptionType>()
                .Property(st => st.Price)
                .HasPrecision(18, 2);

            // Seed initial data
            modelBuilder.Entity<SubscriptionType>().HasData(
                new SubscriptionType { Id = 1, Name = "Daily Personal Training", Description = "Access to personal training for one day", Price = 10.00m, DurationInDays = 1 },
                new SubscriptionType { Id = 2, Name = "Monthly", Description = "Access to all gym facilities for one month", Price = 50.00m, DurationInDays = 30 },
                new SubscriptionType { Id = 3, Name = "3 Months", Description = "Access to all gym facilities for three months", Price = 135.00m, DurationInDays = 90 },
                new SubscriptionType { Id = 4, Name = "6 Months", Description = "Access to all gym facilities for six months", Price = 250.00m, DurationInDays = 180 },
                new SubscriptionType { Id = 5, Name = "Yearly", Description = "Access to all gym facilities for one year", Price = 480.00m, DurationInDays = 365 }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Boxing", Description = "Boxing training" },
                new Service { Id = 2, Name = "Kickboxing", Description = "Kickboxing training" },
                new Service { Id = 3, Name = "Yoga", Description = "Yoga sessions" },
                new Service { Id = 4, Name = "Pilates", Description = "Pilates sessions" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
