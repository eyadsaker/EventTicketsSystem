using Eventsystem.Models;

namespace Eventsystem.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);

        Task<Booking?> GetBookingDetailsAsync(int id);
    }
}