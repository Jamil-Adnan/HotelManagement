using HotelManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement
{
    public class Menu
    {
        private readonly RoomService _roomService;
        private readonly GuestService _guestService;
        private readonly BookingService _bookingService;
        private readonly PaymentService _paymentService;


        public Menu(RoomService roomService, GuestService guestService, BookingService bookingService, PaymentService paymentService)
        {
            _roomService = roomService;
            _guestService = guestService;
            _bookingService = bookingService;
            _paymentService = paymentService;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n====== HOTEL MANAGEMENT SYSTEM ======\n");
                Console.WriteLine("Please press 1 for the guest menu" );
                Console.WriteLine("Please press 2 for the admin menu");
                Console.WriteLine("Please press 3 to exit");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    bool guestLoop = true;
                    while (guestLoop)
                    {
                        Console.WriteLine("\n1. Register Guest");
                        Console.WriteLine("2. View Guest Profile");
                        Console.WriteLine("3. Update Guest Profile");
                        Console.WriteLine("4. Delete Guest Profile");
                        Console.WriteLine("5. Book Room");
                        Console.WriteLine("6. View Booking");
                        Console.WriteLine("7. Change Booking");
                        Console.WriteLine("8. Cancel Booking");
                        Console.WriteLine("9. Check In");
                        Console.WriteLine("10. Check Out");
                        Console.WriteLine("11. Pay Now");
                        Console.WriteLine("12. Pay Later");
                        Console.WriteLine("13. Return To Main Menu");
                        Console.Write("Select an option: ");
                        string guestOption = Console.ReadLine();
                        switch (guestOption)
                        {
                            case "1":
                                _guestService.RegisterGuest();
                                break;
                            case "2":
                                _guestService.ShowGuestProfile();
                                break;
                            case "3":
                                _guestService.UpdateGuestProfile();
                                break;
                            case "4":
                                _guestService.DeleteGuestProfile();
                                break;
                            case "5":
                                _bookingService.BookRoom();
                                break;
                            case "6":
                                _bookingService.ShowBookingDetails();
                                break;
                            case "7":
                                _bookingService.UpdateBooking();
                                break;
                            case "8":
                                _bookingService.CancelBooking();
                                break;
                            case "9":
                                _bookingService.CheckIn();
                                break;
                            case "10":
                                _bookingService.CheckOut();
                                break;
                            case "11":
                                _paymentService.PayNow();
                                break;
                            case "12":
                                _paymentService.PayLater();
                                break;
                            case "13":
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                        guestLoop = false;
                        Console.Clear();
                    }
                }

                else if (choice == "2")
                {
                    bool adminLoop = true;
                    while (adminLoop)
                    {
                        Console.WriteLine("\nWelcome Admin");
                        Console.WriteLine("1. Show All Rooms");
                        Console.WriteLine("2. Create Room");
                        Console.WriteLine("3. Edit Room");
                        Console.WriteLine("4. Delete a room");
                        Console.WriteLine("5. Show All Guests");
                        Console.WriteLine("6. Show All Booking");
                        Console.WriteLine("7. Return To Main Menu");
                        Console.Write("Select an option: ");
                        string adminOption = Console.ReadLine();
                        switch (adminOption)
                        {
                            case "1":
                                _roomService.ShowAllRooms();
                                break;
                            case "2":
                                _roomService.CreateRoom();
                                break;
                            case "3":
                                _roomService.EditRoom();
                                break;
                            case "4":
                                _roomService.DeleteRoom();
                                break;
                            case "5":
                                _guestService.ShowAllGuests();
                                break;
                            case "6":
                                _bookingService.ShowAllBooking();
                                break;
                            case "7":
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                        adminLoop = false;
                        Console.Clear();
                    }                    
                }

                else if (choice == "3")
                {
                    Console.WriteLine("Thank you for staying in our hotel!");
                    return;
                }

                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
                Console.Clear();
            }
        }
    }
}
