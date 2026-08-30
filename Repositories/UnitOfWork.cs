using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Eventsystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        public IEventRepository Events { get; }

        public IGenericRepository<Category> Categories { get; }

        public IGenericRepository<Venue> Venues { get; }

        public IBookingRepository Bookings { get; }

        public IGenericRepository<BookingItem> BookingItems { get; }

        public ITicketTypeRepository TicketTypes { get; }

        public IReviewRepository Reviews { get; }

        public IGenericRepository<ApplicationUser> Users { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Events = new EventRepository(context);

            Categories = new GenericRepository<Category>(context);
            Venues = new GenericRepository<Venue>(context);
            Bookings = new BookingRepository(context);
            BookingItems = new GenericRepository<BookingItem>(context);
            TicketTypes = new TicketTypeRepository(context);
            Reviews = new ReviewRepository(context);
            Users = new GenericRepository<ApplicationUser>(context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _context.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}