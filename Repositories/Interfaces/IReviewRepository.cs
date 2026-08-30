using Eventsystem.Models;

namespace Eventsystem.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsWithDetailsAsync();

        Task<Review?> GetReviewDetailsAsync(int id);
    }
}