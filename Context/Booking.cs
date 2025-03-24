using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Context
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        //public int? PaymentId { get; set; } // Nullable since PaymentId is nullable in the ERD

        public DateTime BookedFrom { get; set; }
        public DateTime BookedTill { get; set; }
        public double TotalCost { get; set; }
        public bool CheckedIn { get; set; } = false;
        public bool Paid { get; set; } = false;

        // Navigation properties
        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
