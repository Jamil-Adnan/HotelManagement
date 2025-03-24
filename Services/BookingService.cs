using HotelManagement.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services
{
    public class BookingService
    {
        private readonly HotelDbContext _context;

        public BookingService(HotelDbContext context)
        {
            _context = context;
        }

        public void BookRoom()
        {
            Console.Write("Enter Guest ID: ");
            if (!int.TryParse(Console.ReadLine(), out int guestId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Guest ID.");
                return;
            }

            Console.Write("Enter Check-in Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkInDate))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Console.Write("Enter Check-out Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOutDate) || checkOutDate <= checkInDate)
            {
                Console.WriteLine("Invalid check-out date.");
                return;
            }

            Console.Write("Enter Room Type (Single/Double): ");
            string roomType = Console.ReadLine().ToLower();
            bool extraBed = false;
            int extraBedCost = 0;

            // If it's a double room, ask if they need an extra bed
            if (roomType == "double")
            {
                Console.Write("Do you want an extra bed? (yes/no): ");
                string extraBedInput = Console.ReadLine().ToLower();
                if (extraBedInput == "yes")
                {
                    Console.Write("Choose extra bed type (single/double): ");
                    string bedType = Console.ReadLine().ToLower();
                    if (bedType == "single")
                    {
                        extraBedCost = 100; // Single bed costs 100 more
                    }
                    else if (bedType == "double")
                    {
                        extraBedCost = 200; // Double bed costs 200 more
                    }
                    else
                    {
                        Console.WriteLine("Invalid bed type. Please enter 'single' or 'double'.");
                        return;
                    }
                }
            }

            // Fetch available room
            var availableRoom = _context.Rooms.FirstOrDefault(r => r.RoomType.ToLower() == roomType && r.Availability);
            if (availableRoom == null)
            {
                Console.WriteLine("No available rooms of this type.");
                return;
            }

            // Calculate price per night (with extra bed charge for double rooms)
            double pricePerNight = roomType == "single" ? 399 : 599 + extraBedCost;

            // Calculate the total cost
            double totalCost = pricePerNight * (checkOutDate - checkInDate).Days;

            // Create a new booking
            var newBooking = new Booking
            {
                GuestId = guestId,
                RoomId = availableRoom.RoomId,
                BookedFrom = checkInDate,
                BookedTill = checkOutDate,
                TotalCost = totalCost,
                CheckedIn = false,
                Paid = false // Initially, the booking is not paid
            };

            // Mark room as unavailable
            availableRoom.Availability = false;

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            // Display the booking details
            Console.WriteLine("\nBooking successfully created!");
            Console.WriteLine($"Booking ID: {newBooking.BookingId}");
            Console.WriteLine($"Room ID: {availableRoom.RoomId}");
            Console.WriteLine($"Guest ID: {newBooking.GuestId}");
            Console.WriteLine($"Room Type: {availableRoom.RoomType}");
            Console.WriteLine($"Extra Bed: {(extraBed ? (extraBedCost == 100 ? "Single (+$100)" : "Double (+$200)") : "None")}");
            Console.WriteLine($"Check-in Date: {checkInDate:yyyy-MM-dd}");
            Console.WriteLine($"Check-out Date: {checkOutDate:yyyy-MM-dd}");
            Console.WriteLine($"Total Cost: {totalCost:C}");
            Console.WriteLine($"Is Paid: {(newBooking.Paid ? "Yes" : "No")}");  // Print whether it's paid or not

            Console.Read();
            Console.Clear();
        }

        public void ShowBookingDetails()
        {
            Console.Write("Enter Booking ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var booking = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            // Display the booking details
            Console.WriteLine($"\nBooking Details for Booking ID: {booking.BookingId}");
            Console.WriteLine($"Booking ID: {booking.BookingId}");
            Console.WriteLine($"Guest ID: {booking.GuestId}");
            Console.WriteLine($"Guest: {booking.Guest.FirstName} {booking.Guest.LastName}");
            Console.WriteLine($"Room ID: {booking.RoomId}");
            Console.WriteLine($"Room Type: {booking.Room.RoomType}");
            Console.WriteLine($"Check-in Date: {booking.BookedFrom:yyyy-MM-dd}");
            Console.WriteLine($"Check-out Date: {booking.BookedTill:yyyy-MM-dd}");
            Console.WriteLine($"Total Cost: {booking.TotalCost:C}");
            Console.WriteLine($"Checked In: {(booking.CheckedIn ? "Yes" : "No")}");
            Console.WriteLine($"Is Paid: {(booking.Paid ? "Yes" : "No")}");  // Print whether it's paid or not

            Console.Read();
            Console.Clear();
        }

        public void UpdateBooking()
        {
            Console.Write("Enter Booking ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            if (booking.CheckedIn)
            {
                Console.WriteLine("Cannot update booking. Guest has already checked in.");
                return;
            }

            Console.Write("Enter new Check-in Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkInDate))
            {
                Console.WriteLine("Invalid date.");
                return;
            }

            Console.Write("Enter new Check-out Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime checkOutDate) || checkOutDate <= checkInDate)
            {
                Console.WriteLine("Invalid check-out date.");
                return;
            }

            // Recalculate the price based on the updated check-in and check-out dates
            string roomType = booking.Room.RoomType.ToLower();
            bool extraBed = booking.Room.ExtraBeds;
            int extraBedCost = 0;

            if (roomType == "double" && extraBed)
            {
                Console.Write("Do you want an extra bed? (yes/no): ");
                string extraBedInput = Console.ReadLine().ToLower();
                if (extraBedInput == "yes")
                {
                    Console.Write("Choose extra bed type (single/double): ");
                    string bedType = Console.ReadLine().ToLower();
                    if (bedType == "single")
                    {
                        extraBedCost = 100; // Single bed costs 100 more
                    }
                    else if (bedType == "double")
                    {
                        extraBedCost = 200; // Double bed costs 200 more
                    }
                    else
                    {
                        Console.WriteLine("Invalid bed type. Please enter 'single' or 'double'.");
                        return;
                    }
                }
            }

            // Calculate the new total cost based on room type and extra bed
            double pricePerNight = roomType == "single" ? 399 : 599 + extraBedCost;
            double totalCost = pricePerNight * (checkOutDate - checkInDate).Days;

            // Update the booking with the new details
            booking.BookedFrom = checkInDate;
            booking.BookedTill = checkOutDate;
            booking.TotalCost = totalCost;

            _context.SaveChanges();

            // Display the updated booking details
            Console.WriteLine("\nBooking successfully updated!");
            Console.WriteLine($"Booking ID: {booking.BookingId}");
            Console.WriteLine($"Room ID: {booking.RoomId}");
            Console.WriteLine($"Guest ID: {booking.GuestId}");
            Console.WriteLine($"Room Type: {booking.Room.RoomType}");
            Console.WriteLine($"Extra Bed: {(extraBed ? (extraBedCost == 100 ? "Single (+$100)" : "Double (+$200)") : "None")}");
            Console.WriteLine($"Check-in Date: {checkInDate:yyyy-MM-dd}");
            Console.WriteLine($"Check-out Date: {checkOutDate:yyyy-MM-dd}");
            Console.WriteLine($"Total Cost: {totalCost:C}");
            Console.WriteLine($"Is Paid: {(booking.Paid ? "Yes" : "No")}");  // Print whether it's paid or not

            Console.Read();
            Console.Clear();
        }

        public void CancelBooking()
        {
            Console.Write("Enter Booking ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            if (booking.CheckedIn)
            {
                Console.WriteLine("Cannot delete booking. Guest has already checked in.");
                return;
            }

            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
            if (room != null)
            {
                room.Availability = true; // Mark room as available again
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            Console.WriteLine("Booking deleted successfully.");

            Console.Read();
            Console.Clear();
        }

        public void CheckIn()
        {
            Console.Write("Enter Booking ID for Check-In: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Booking ID.");
                return;
            }

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            if (booking.BookedFrom.Date != DateTime.Today)
            {
                Console.WriteLine("You can only check in on your booking start date.");
                return;
            }

            booking.CheckedIn = true;
            _context.SaveChanges();
            Console.WriteLine("Check-in successful. Enjoy your stay!");

            Console.Read();
            Console.Clear();
        }

        // 2️⃣ Guest Check-Out
        public void CheckOut()
        {
            Console.Write("Enter Booking ID for Check-Out: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Booking ID.");
                return;
            }

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            if (!booking.CheckedIn)
            {
                Console.WriteLine("Guest has not checked in yet. Check-in is required before check-out.");
                return;
            }

            booking.CheckedIn = false;  // Reset check-in status before deleting
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            Console.WriteLine("Check-out successful. Thank you for staying with us!");

            Console.Read();
            Console.Clear();
        }

        public void ShowAllBooking()
        {
            var bookings = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .ToList();

            if (bookings.Any())
            {
                Console.WriteLine("All Bookings:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingId}, " +
                                      $"Guest: {booking.Guest.FirstName} {booking.Guest.LastName}, " +
                                      $"Room Type: {booking.Room.RoomType}, " +
                                      $"From: {booking.BookedFrom:yyyy-MM-dd} To: {booking.BookedTill:yyyy-MM-dd}, " +
                                      $"Paid: {(booking.Paid ? "Yes" : "No")}");
                }
            }
            else
            {
                Console.WriteLine("No bookings found.");
            }
        }
    }
}

