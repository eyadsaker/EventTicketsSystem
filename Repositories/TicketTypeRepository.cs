using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Repositories
{
    public class TicketTypeRepository : GenericRepository<TicketType>, ITicketTypeRepository
    {
        public TicketTypeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<TicketType>> GetTicketTypesWithEventAsync()
        {
            return await _context.TicketTypes
                .Include(t => t.Event)
                .ToListAsync();
        }

        public async Task<TicketType?> GetTicketTypeDetailsAsync(int id)
        {
            return await _context.TicketTypes
                .Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}