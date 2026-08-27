using Eventsystem.Models;

namespace Eventsystem.ViewModel
{
    public class EventDetailsVM
    {
        public Event Event { get; set; } = null!;
        public List<TicketTypeVM> TicketTypes { get; set; } = new();
    }

    public class TicketTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; } 
        public int SelectedQuantity { get; set; }  
    }
}
