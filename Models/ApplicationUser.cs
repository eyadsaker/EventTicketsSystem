using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Eventsystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? CompanyName { get; set; }

        public bool IsBlocked { get; set; } = false;

        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
