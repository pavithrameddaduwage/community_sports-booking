using CommunitySportsBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunitySportsBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Facility> Facilities { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Facility)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Member)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Facility)
                .WithMany(f => f.Reviews)
                .HasForeignKey(r => r.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data
            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    MemberId = 1,
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    Password = "1234",
                    ContactNumber = "07123456789",
                    Address = "12 High Street, Colombo",
                    PreferredSports = "Tennis, Soccer",
                    Role = "Admin",
                    CreatedAt = DateTime.Parse("2025-01-01")
                },
                new Member
                {
                    MemberId = 2,
                    FullName = "Sarah Jenkins",
                    Email = "sarah@example.com",
                    Password = "password123",
                    ContactNumber = "07987654321",
                    Address = "45 Park Avenue, Kandy",
                    PreferredSports = "Basketball, Badminton",
                    Role = "Member",
                    CreatedAt = DateTime.Parse("2025-01-02")
                }
            );

            modelBuilder.Entity<Facility>().HasData(
                new Facility
                {
                    FacilityId = 1,
                    Name = "Torrington Olympic Tennis Court A",
                    FacilityType = "Tennis",
                    Location = "Colombo 07 (Torrington Grounds)",
                    HourlyRate = 2500.00m,
                    Capacity = 4,
                    Description = "Professional outdoor hard court with night floodlights, seating, and player pavilion.",
                    ImageUrl = "https://images.unsplash.com/photo-1595435934249-5df7ed86e1c0?auto=format&fit=crop&w=800&q=80"
                },
                new Facility
                {
                    FacilityId = 2,
                    Name = "Racecourse 3G Football Pitch",
                    FacilityType = "Soccer",
                    Location = "Colombo 07 (Racecourse Grounds)",
                    HourlyRate = 5500.00m,
                    Capacity = 14,
                    Description = "Full size 7-a-side 3G synthetic turf suitable for all weather tournament matches.",
                    ImageUrl = "https://images.unsplash.com/photo-1529900748604-07564a03e7a6?auto=format&fit=crop&w=800&q=80"
                },
                new Facility
                {
                    FacilityId = 3,
                    Name = "Sugathadasa Indoor Basketball Arena",
                    FacilityType = "Basketball",
                    Location = "Colombo 13 (Kotahena)",
                    HourlyRate = 4000.00m,
                    Capacity = 10,
                    Description = "Indoor maple hardwood court with adjustable hoops, digital scoreboard, and spectator stands.",
                    ImageUrl = "https://images.unsplash.com/photo-1546519638-68e109498ffc?auto=format&fit=crop&w=800&q=80"
                },
                new Facility
                {
                    FacilityId = 4,
                    Name = "Havelock Town Badminton Court 1",
                    FacilityType = "Badminton",
                    Location = "Colombo 05 (Havelock Town)",
                    HourlyRate = 1800.00m,
                    Capacity = 4,
                    Description = "Indoor court with non-slip sprung wood flooring and tournament-grade netting.",
                    ImageUrl = "https://images.unsplash.com/photo-1626224583764-f87db24ac4ea?auto=format&fit=crop&w=800&q=80"
                },
                new Facility
                {
                    FacilityId = 5,
                    Name = "CR & FC Municipal Swimming Pool",
                    FacilityType = "Swimming",
                    Location = "Colombo 07 (Cinnamon Gardens)",
                    HourlyRate = 3000.00m,
                    Capacity = 20,
                    Description = "25m heated indoor lap pool with designated fast and slow lanes and certified lifeguards.",
                    ImageUrl = "https://images.unsplash.com/photo-1576013551627-0cc20b96c2a7?auto=format&fit=crop&w=800&q=80"
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    MemberId = 1,
                    FacilityId = 1,
                    BookingDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 - 11:00",
                    Status = "Confirmed",
                    TotalPrice = 2500.00m,
                    CreatedAt = DateTime.Now
                }
            );

            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    ReviewId = 1,
                    MemberId = 1,
                    FacilityId = 1,
                    Rating = 5,
                    Comment = "Fantastic tennis court! The floodlights made evening play super clear.",
                    ReviewDate = DateTime.Now.AddDays(-2)
                },
                new Review
                {
                    ReviewId = 2,
                    MemberId = 2,
                    FacilityId = 3,
                    Rating = 4,
                    Comment = "Great basketball court, clean changing rooms. Would definitely book again.",
                    ReviewDate = DateTime.Now.AddDays(-1)
                }
            );
        }
    }
}
