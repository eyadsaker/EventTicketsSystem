using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetReviewsWithDetailsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Event)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewDetailsAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Event)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}