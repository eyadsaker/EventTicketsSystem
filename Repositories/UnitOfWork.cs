using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;

namespace Eventsystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEventRepository Events { get; }

        public IGenericRepository<Category> Categories { get; }

        public IGenericRepository<Venue> Venues { get; }

        public IGenericRepository<Booking> Bookings { get; }

        public IGenericRepository<TicketType> TicketTypes { get; }

        public IGenericRepository<Review> Reviews { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Events = new EventRepository(context);

            Categories = new GenericRepository<Category>(context);
            Venues = new GenericRepository<Venue>(context);
            Bookings = new GenericRepository<Booking>(context);
            TicketTypes = new GenericRepository<TicketType>(context);
            Reviews = new GenericRepository<Review>(context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}