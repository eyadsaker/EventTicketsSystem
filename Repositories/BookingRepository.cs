using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Items)
                    .ThenInclude(i => i.TicketType)
                        .ThenInclude(t => t!.Event)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingDetailsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.TicketType)
                        .ThenInclude(t => t!.Event)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}