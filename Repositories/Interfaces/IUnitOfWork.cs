using Microsoft.EntityFrameworkCore.Storage;

namespace Eventsystem.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }

        IGenericRepository<Models.Category> Categories { get; }

        IGenericRepository<Models.Venue> Venues { get; }

        IBookingRepository Bookings { get; }

        IGenericRepository<Models.BookingItem> BookingItems { get; }

        ITicketTypeRepository TicketTypes { get; }

        IReviewRepository Reviews { get; }

        IGenericRepository<Models.ApplicationUser> Users { get; }

        Task<int> SaveAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}