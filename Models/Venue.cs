using System.ComponentModel.DataAnnotations;

namespace Eventsystem.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string City { get; set; } = string.Empty;

        [Range(1, 100000)]
        public int Capacity { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
