using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventsystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string BookingReference { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

        public DateTime? CheckedInAt { get; set; }

        public ICollection<BookingItem> Items { get; set; } = new List<BookingItem>();
    }
}
