using HotelManagement.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services
{
    public class PaymentService
    {
        private readonly HotelDbContext _context;

        public PaymentService(HotelDbContext context)
        {
            _context = context;
        }

        public void PayNow()
        {
            Console.Write("Enter Booking ID to make payment: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Booking ID.");
                return;
            }

            var booking = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Include(b => b.Payment)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            // Check if the booking has already been paid
            if (booking.Paid)
            {
                Console.WriteLine("This booking has already been paid.");
                return;
            }

            // Get payment method from the user
            Console.Write("Enter payment method (Credit Card / Cash / PayPal / Any Other Method): ");
            string paymentMethod = Console.ReadLine();

            // Confirm payment
            Console.Write("Confirm payment? (yes/no): ");
            string confirm = Console.ReadLine().ToLower();
            if (confirm != "yes")
            {
                Console.WriteLine("Payment canceled.");
                return;
            }

            // If no payment exists for this booking, create a new payment record
            if (booking.Payment == null)
            {
                booking.Payment = new Payment
                {
                    PaymentDate = DateTime.Today,
                    //Paid = true,
                    PaymentMethod = paymentMethod,
                    //Amount = booking.TotalCost,
                    BookingId = booking.BookingId
                };

                _context.Payments.Add(booking.Payment);
            }
            else
            {
                // Update existing payment record
                booking.Payment.PaymentDate = DateTime.Today;
                //booking.Payment.Paid = true;
                booking.Payment.PaymentMethod = paymentMethod;
                //booking.Payment.Amount = booking.TotalCost;
            }

            // Update the IsPaid property of the booking
            booking.Paid = true;

            // Save the changes to the database
            _context.SaveChanges();
            Console.WriteLine($"Payment successful! Payment ID: {booking.Payment.PaymentId}");

            Console.Read();
            Console.Clear();
        }

        public void PayLater()
        {
            Console.Write("Enter Booking ID to defer payment: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Booking ID.");
                return;
            }

            var booking = _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Include(b => b.Payment)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            DateTime today = DateTime.Today;
            DateTime paymentDueDate = booking.BookedFrom.AddDays(-2);  // Payment is due 2 days before booking start date

            // If the booking start date is more than 3 days away from today, display booking details
            if (booking.BookedFrom > today.AddDays(3))
            {
                Console.WriteLine("\n--- Booking Details ---");
                Console.WriteLine($"Booking ID: {booking.BookingId}");
                Console.WriteLine($"Guest: {booking.Guest.FirstName} {booking.Guest.LastName}");
                Console.WriteLine($"Room ID: {booking.RoomId}");
                Console.WriteLine($"Room Type: {booking.Room.RoomType}");
                Console.WriteLine($"Check-in Date: {booking.BookedFrom:yyyy-MM-dd}");
                Console.WriteLine($"Check-out Date: {booking.BookedTill:yyyy-MM-dd}");
                Console.WriteLine($"Total Cost: {booking.TotalCost:C}");
                Console.WriteLine($"\nPlease make sure to complete the payment before: {paymentDueDate:yyyy-MM-dd}, to keep your booking!");
            }
            else
            {
                Console.WriteLine("\nPlease make an immediate payment to keep your booking.");
                Console.Write("Enter payment method (Credit Card / Cash / PayPal): ");
                string paymentMethod = Console.ReadLine();

                // Confirm payment
                Console.Write("Confirm payment? (yes/no): ");
                string confirm = Console.ReadLine().ToLower();
                if (confirm == "yes")
                {
                    booking.Payment = new Payment
                    {
                        PaymentDate = DateTime.Now,
                        //Paid = true,
                        PaymentMethod = paymentMethod,
                        //Amount = booking.TotalCost,
                        BookingId = booking.BookingId
                    };

                    _context.Payments.Add(booking.Payment);
                    booking.Paid = true; // Mark the booking as paid
                    _context.SaveChanges();
                    Console.WriteLine("Payment successful! Your booking has been confirmed.");
                    return;
                }
                else
                {
                    Console.WriteLine("Payment canceled. The booking will be automatically canceled if no payment is made in the next 2 days.");
                }
            }

            // Automatically cancel the booking if payment is not completed 2 days before the start date
            if (today >= paymentDueDate)
            {
                // Cancel the booking and make the room available again
                var room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
                if (room != null)
                {
                    room.Availability = true; // Make the room available again
                }

                _context.Bookings.Remove(booking);
                _context.SaveChanges();
                Console.WriteLine("Booking has been automatically canceled due to non-payment.");
            }

            Console.Read();
            Console.Clear();
        }
    }
}
