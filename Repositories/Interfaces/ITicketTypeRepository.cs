using Eventsystem.Models;

namespace Eventsystem.Repositories.Interfaces
{
    public interface ITicketTypeRepository : IGenericRepository<TicketType>
    {
        Task<IEnumerable<TicketType>> GetTicketTypesWithEventAsync();

        Task<TicketType?> GetTicketTypeDetailsAsync(int id);
    }
}