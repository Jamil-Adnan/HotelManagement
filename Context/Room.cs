using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Context
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomType { get; set; } // "Single" or "Double"
        public bool ExtraBeds { get; set; } // true = 1 extra bed, false = no extra bed
        public double PricePerNight { get; set; } // Public setter allows updates
        public bool Availability { get; set; } = true;
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public Room() { }

        public Room(string roomType, bool extraBeds)
        {
            RoomType = roomType;
            ExtraBeds = extraBeds;
            UpdatePrice(); // Set price on creation
        }

        // Method to update price dynamically based on room type and extra bed
        public void UpdatePrice()
        {
            PricePerNight = (RoomType.ToLower() == "single") ? 399 : (ExtraBeds ? 699 : 599);
        }
    }
}
