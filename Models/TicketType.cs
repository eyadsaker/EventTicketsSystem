using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventsystem.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Range(1, 100000)]
        public int Quantity { get; set; }

        public int SoldQuantity { get; set; } = 0;

     
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

        [NotMapped]
        public int Available => Quantity - SoldQuantity;
    }
}