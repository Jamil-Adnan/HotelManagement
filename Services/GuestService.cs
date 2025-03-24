using HotelManagement.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services
{
    public class GuestService
    {
        private readonly HotelDbContext _context;

        public GuestService(HotelDbContext context)
        {
            _context = context;
        }

        // Method to display all guests
        public void ShowAllGuests()
        {
            var guests = _context.Guests.ToList();

            if (guests.Count == 0)
            {
                Console.WriteLine("No guests found.");
                return;
            }

            Console.WriteLine("\nList of Guests:");
            foreach (var guest in guests)
            {
                Console.WriteLine($"Guest ID: {guest.GuestId}, Name: {guest.FirstName} {guest.LastName}, Country: {guest.Country}, Email: {guest.Email}, Phone: {guest.Phone}");
            }

            Console.Read();
            Console.Clear();
        }

        // Method to register a new guest
        public void RegisterGuest()
        {
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Country: ");
            string country = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();

            // Create a new guest object
            var newGuest = new Guest
            {
                FirstName = firstName,
                LastName = lastName,
                Country = country,
                Email = email,
                Phone = phone
            };

            // Add the guest to the database
            _context.Guests.Add(newGuest);
            _context.SaveChanges();

            Console.WriteLine($"Guest registered successfully! Guest ID: {newGuest.GuestId}");

            Console.Read();
            Console.Clear();
        }

        public void ShowGuestProfile()
        {
            Console.Write("Enter Guest ID: ");
            if (int.TryParse(Console.ReadLine(), out int guestId))
            {
                var guest = _context.Guests.FirstOrDefault(g => g.GuestId == guestId);
                if (guest == null)
                {
                    Console.WriteLine($"Guest with ID {guestId} not found.");
                    return;
                }

                Console.WriteLine("\nGuest Profile:");
                Console.WriteLine($"Guest ID: {guest.GuestId}");
                Console.WriteLine($"Name: {guest.FirstName} {guest.LastName}");
                Console.WriteLine($"Country: {guest.Country}");
                Console.WriteLine($"Email: {guest.Email}");
                Console.WriteLine($"Phone: {guest.Phone}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Guest ID.");
            }

            Console.Read();
            Console.Clear();
        }

        public void UpdateGuestProfile()
        {
            Console.Write("Enter Guest ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int guestId))
            {
                var guest = _context.Guests.FirstOrDefault(g => g.GuestId == guestId);
                if (guest == null)
                {
                    Console.WriteLine($"Guest with ID {guestId} not found.");
                    return;
                }

                Console.WriteLine($"Updating profile for Guest ID: {guestId}");

                Console.Write("Enter new First Name (leave empty to keep current): ");
                string firstName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(firstName)) guest.FirstName = firstName;

                Console.Write("Enter new Last Name (leave empty to keep current): ");
                string lastName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(lastName)) guest.LastName = lastName;

                Console.Write("Enter new Country (leave empty to keep current): ");
                string country = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(country)) guest.Country = country;

                Console.Write("Enter new Email (leave empty to keep current): ");
                string email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email)) guest.Email = email;

                Console.Write("Enter new Phone (leave empty to keep current): ");
                string phone = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(phone)) guest.Phone = phone;

                // Save changes
                _context.SaveChanges();
                Console.WriteLine("Guest profile updated successfully!");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Guest ID.");
            }

            Console.Read();
            Console.Clear();
        }

        public void DeleteGuestProfile()
        {
            Console.Write("Enter Guest ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int guestId))
            {
                var guest = _context.Guests.FirstOrDefault(g => g.GuestId == guestId);
                if (guest == null)
                {
                    Console.WriteLine($"Guest with ID {guestId} not found.");
                    return;
                }

                _context.Guests.Remove(guest);
                _context.SaveChanges();
                Console.WriteLine($"Guest profile with ID {guestId} deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Guest ID.");
            }

            Console.Read();
            Console.Clear();
        }
    }
}
