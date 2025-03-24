using HotelManagement.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public HotelDbContext()
        {
        }

        public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Guest)
                .WithMany(g => g.Bookings)
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Adjusted for non-nullable BookingId in Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // Or Restrict, depending on your intent

            modelBuilder.Entity<Room>().HasData(
            new Room { RoomId = 1, RoomType = "Single", ExtraBeds = false, Availability = true, PricePerNight = 399.00 },
            new Room { RoomId = 2, RoomType = "Double", ExtraBeds = true, Availability = false, PricePerNight = 699.00 }, // Unavailable Room
            new Room { RoomId = 3, RoomType = "Double", ExtraBeds = false, Availability = true, PricePerNight = 599.00 },
            new Room { RoomId = 4, RoomType = "Single", ExtraBeds = false, Availability = false, PricePerNight = 399.00 }
            );

            modelBuilder.Entity<Guest>().HasData(
            new Guest { GuestId = 1, FirstName = "Iron", LastName = "Maiden", Country = "England", Email = "ironmaiden@email.com", Phone = "0044-12345678" },
            new Guest { GuestId = 2, FirstName = "Sixfeet", LastName = "Under", Country = "USA", Email = "sixfeetunder@email.com", Phone = "001-12345678" },
            new Guest { GuestId = 3, FirstName = "Insomnium", LastName = "NA", Country = "Finland", Email = "insomnium@email.com", Phone = "00358-123456" },
            new Guest { GuestId = 4, FirstName = "Opeth", LastName = "NA", Country = "Sweden", Email = "opeth@email.com", Phone = "0046-123456" }
            );

            modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                BookingId = 1,
                GuestId = 1,
                RoomId = 1,
                BookedFrom = new DateTime(2025,4,1),
                BookedTill = new DateTime(2025,4,4),
                TotalCost = 399.00 * 3,
                Paid = true
            },
            new Booking
            {
                BookingId = 2,
                GuestId = 2,
                RoomId = 2,
                BookedFrom = new DateTime(2025,3,24),
                BookedTill = new DateTime(2025,4,2),
                TotalCost = 699.00 * 9,
                Paid = true,
                CheckedIn = true
            },
            new Booking
            {
                BookingId = 3,
                GuestId = 3,
                RoomId = 3,
                BookedFrom = new DateTime(2025,4,5),
                BookedTill = new DateTime(2025,4,12),
                TotalCost = 599.00 * 7,
                Paid = false
            },
            new Booking
            {
                BookingId = 4,
                GuestId = 4,
                RoomId = 1,
                BookedFrom = new DateTime(2025,3,22),
                BookedTill = new DateTime(2025,4,6),
                TotalCost = 399.00 * 15,
                Paid = true,
                CheckedIn = true
            }
            );

            // Seeding 4 Payments
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentId = 1,
                    PaymentDate = new DateTime(2025,3,28),
                    PaymentMethod = "Credit Card",
                    BookingId = 1
                },
                new Payment
                {
                    PaymentId = 2,
                    PaymentDate = new DateTime(2025,3,20),
                    PaymentMethod = "PayPal",
                    BookingId = 2
                },
                new Payment
                {
                    PaymentId = 3,
                    PaymentDate = null,
                    PaymentMethod = null,
                    BookingId = 3
                },
                new Payment
                {
                    PaymentId = 4,
                    PaymentDate = new DateTime(2025,3,15),
                    PaymentMethod = "Credit Card",
                    BookingId = 4
                }
                );
        }
    }
}
