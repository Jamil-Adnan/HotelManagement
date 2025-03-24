using HotelManagement.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services
{
    public class RoomService
    {
        private readonly HotelDbContext _context;

        public RoomService(HotelDbContext context)
        {
            _context = context;
        }


        public void CreateRoom()
        {
            // Get the current count of single and double rooms in the database
            int singleRoomCount = _context.Rooms.Count(r => r.RoomType == "single");
            int doubleRoomCount = _context.Rooms.Count(r => r.RoomType == "double");

            Console.WriteLine("Enter Room Type (Single or Double): ");
            string roomType = Console.ReadLine()?.ToLower();

            // Validate room type input
            if (roomType != "single" && roomType != "double")
            {
                Console.WriteLine("Invalid room type. Please enter 'Single' or 'Double'.");
                return;
            }

            // Check for room type capacity
            if (roomType == "single" && singleRoomCount >= 4)
            {
                Console.WriteLine("Cannot create more single rooms. The maximum limit of 4 single rooms has been reached.");
                return;
            }
            if (roomType == "double" && doubleRoomCount >= 6)
            {
                Console.WriteLine("Cannot create more double rooms. The maximum limit of 6 double rooms has been reached.");
                return;
            }

            bool extraBeds = false;
            int extraBedCost = 0;

            // Only ask for extra beds if it's a double room
            if (roomType == "double")
            {
                Console.WriteLine("Does the room need an extra bed? (Yes or No): ");
                string extraBedsInput = Console.ReadLine().ToLower();

                if (extraBedsInput == "yes")
                {
                    Console.WriteLine("What type of extra bed? (single or double): ");
                    string bedType = Console.ReadLine().ToLower();

                    if (bedType == "single")
                    {
                        extraBeds = true;
                        extraBedCost = 100; // Single bed costs 100 more
                    }
                    else if (bedType == "double")
                    {
                        extraBeds = true;
                        extraBedCost = 200; // Double bed costs 200 more
                    }
                    else
                    {
                        Console.WriteLine("Invalid bed type. Please enter 'single' or 'double'.");
                        return;
                    }
                }
                else if (extraBedsInput != "no")
                {
                    Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                    return;
                }
            }

            // Ask for room availability
            Console.WriteLine("Is the room available? (Yes or No): ");
            string availabilityInput = Console.ReadLine().ToLower();
            bool availability = availabilityInput == "yes";

            // Set the room price based on type and extra bed
            double pricePerNight = roomType == "single" ? 399 : 599 + extraBedCost;

            // Create and save the new room
            Room newRoom = new Room
            {
                RoomType = roomType,
                ExtraBeds = extraBeds,
                Availability = availability,
                PricePerNight = pricePerNight
            };

            _context.Rooms.Add(newRoom);
            _context.SaveChanges();

            // Inform user of the room details
            Console.WriteLine($"Room created successfully! Room ID: {newRoom.RoomId}, Type: {newRoom.RoomType}, Price per Night: {newRoom.PricePerNight} SEK, Extra Beds: {newRoom.ExtraBeds}, Availability: {newRoom.Availability}");
            Console.Read();
            Console.Clear();
        }


        public void ShowAllRooms()
        {
            var rooms = _context.Rooms.ToList(); // Fetch all rooms from the database

            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms available.");
                return;
            }

            Console.WriteLine("\nList of All Rooms:");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var room in rooms)
            {
                Console.WriteLine($"Room ID: {room.RoomId}, Room Type: {room.RoomType}, Extra Bed: {(room.ExtraBeds ? "Yes (+$100/night)" : "No")}, Price per Night: {room.PricePerNight:C}, Availability: {(room.Availability ? "Available" : "Occupied")}");
            }

            Console.Read();
            Console.Clear();
        }

        public void DeleteRoom()
        {
            Console.Write("Enter the Room ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric Room ID.");
                return;
            }

            var room = _context.Rooms.Include(r => r.Bookings).FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                Console.WriteLine($"Room with ID {roomId} does not exist.");
                return;
            }

            if (room.Bookings != null && room.Bookings.Count > 0)
            {
                Console.WriteLine($"Room ID {roomId} has existing bookings and cannot be deleted.");
                return;
            }

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            Console.WriteLine($"Room ID {roomId} has been successfully deleted.");

            Console.Read();
            Console.Clear();
        }

        public void EditRoom()
        {
            // Prompt for Room ID
            Console.Write("Enter the Room ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric Room ID.");
                return;
            }

            // Find the room in the database
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room == null)
            {
                Console.WriteLine($"Room with ID {roomId} does not exist.");
                return;
            }

            // Show current room details
            Console.WriteLine($"Current details for Room ID {roomId}:");
            Console.WriteLine($"Room Type: {room.RoomType}, Price per Night: {room.PricePerNight}, Extra Beds: {room.ExtraBeds}, Availability: {room.Availability}");

            // Ask for new room type
            Console.WriteLine("Enter new Room Type (Single or Double): ");
            string roomType = Console.ReadLine().ToLower();

            // Validate room type input
            if (roomType != "single" && roomType != "double")
            {
                Console.WriteLine("Invalid room type. Please enter 'Single' or 'Double'.");
                return;
            }

            bool extraBeds = false;
            int extraBedCost = 0;

            // Ask if extra bed is needed (only for double rooms)
            if (roomType == "double")
            {
                Console.WriteLine("Does the room need an extra bed? (Yes or No): ");
                string extraBedsInput = Console.ReadLine().ToLower();

                if (extraBedsInput == "yes")
                {
                    Console.WriteLine("What type of extra bed? (single or double): ");
                    string bedType = Console.ReadLine().ToLower();

                    if (bedType == "single")
                    {
                        extraBeds = true;
                        extraBedCost = 100; // Single bed costs 100 more
                    }
                    else if (bedType == "double")
                    {
                        extraBeds = true;
                        extraBedCost = 200; // Double bed costs 200 more
                    }
                    else
                    {
                        Console.WriteLine("Invalid bed type. Please enter 'single' or 'double'.");
                        return;
                    }
                }
                else if (extraBedsInput != "no")
                {
                    Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                    return;
                }
            }

            // Ask for new room availability
            Console.WriteLine("Is the room available? (Yes or No): ");
            string availabilityInput = Console.ReadLine().ToLower();
            bool availability = availabilityInput == "yes";

            // Update room properties and price
            room.RoomType = roomType;
            room.ExtraBeds = extraBeds;
            room.Availability = availability;
            room.PricePerNight = roomType == "single" ? 399 : 599 + extraBedCost;

            _context.Rooms.Update(room);
            _context.SaveChanges();

            // Inform user of the updated room details
            Console.WriteLine($"Room ID {roomId} updated successfully! Room Type: {room.RoomType}, Price per Night: {room.PricePerNight} SEK, Extra Beds: {room.ExtraBeds}, Availability: {room.Availability}");
            Console.Read();
            Console.Clear();
        }
    }
}
