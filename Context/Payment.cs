using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Context
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }

        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
