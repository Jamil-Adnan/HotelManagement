using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Context
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        // Navigation property for Bookings (one-to-many)
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
