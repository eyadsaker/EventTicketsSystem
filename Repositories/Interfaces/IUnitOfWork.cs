namespace Eventsystem.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }

        IGenericRepository<Models.Category> Categories { get; }

        IGenericRepository<Models.Venue> Venues { get; }

        IGenericRepository<Models.Booking> Bookings { get; }

        IGenericRepository<Models.TicketType> TicketTypes { get; }

        IGenericRepository<Models.Review> Reviews { get; }

        Task<int> SaveAsync();
    }
}