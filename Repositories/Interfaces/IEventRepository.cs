using Eventsystem.Models;

namespace Eventsystem.Repositories.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsWithDetailsAsync();

        Task<Event?> GetEventDetailsAsync(int id);
    }
}