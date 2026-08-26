using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventsystem.Models
{
    public class BookingItem
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

        public int TicketTypeId { get; set; }
        public TicketType? TicketType { get; set; }

        [Range(1, 10)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal SubTotal => Quantity * UnitPrice;
    }
}
