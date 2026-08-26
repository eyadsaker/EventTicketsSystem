using System.ComponentModel.DataAnnotations;

namespace Eventsystem.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
