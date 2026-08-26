using System.ComponentModel.DataAnnotations;

namespace Eventsystem.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int VenueId { get; set; }
        public Venue? Venue { get; set; }

        [Required]
        public string OrganizerId { get; set; } = string.Empty;
        public ApplicationUser? Organizer { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? ImageUrl { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
