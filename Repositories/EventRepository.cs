using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Repositories
{
    public class EventRepository
        : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsWithDetailsAsync()
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .ToListAsync();
        }

        public async Task<Event?> GetEventDetailsAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .Include(e => e.TicketTypes)
                .Include(e => e.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}